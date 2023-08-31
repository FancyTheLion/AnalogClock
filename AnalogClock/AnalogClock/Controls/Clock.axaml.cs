using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Data;
using System.Globalization;
using System.Reflection.Metadata;
using System.Xml.Schema;

namespace AnalogClock.Controls
{
    public partial class Clock : UserControl
    {
        /// <summary>
        /// ������ ������
        /// </summary>
        private const double NotchTextSize = 30;

        /// <summary>
        /// ���� ����
        /// </summary>
        private readonly Color NotchTextColor = Colors.Black;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ���������� ������� �������
        /// </summary>
        private const double HoursHandStartRadius = 0;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ������������� ������� �������
        /// </summary>
        private const double HoursHandEndRadius = 0.5;

        /// <summary>
        /// ������� ������� �������
        /// </summary>
        private const double HoursArrowThickness = 10;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ���������� �������� �������
        /// </summary>
        private const double MinutesHandStartRadius = 0;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ������������� �������� �������
        /// </summary>
        private const double MinutesHandEndRadius = 0.75;

        /// <summary>
        /// ������� �������� �������
        /// </summary>
        private const double MinutesArrowThickness = 5;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ���������� ��������� �������
        /// </summary>
        private const double SecondsHandStartRadius = 0;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ������������� ��������� �������
        /// </summary>
        private const double SecondsHandEndRadius = 0.90;

        /// <summary>
        /// ������� ��������� �������
        /// </summary>
        private const double SecondsArrowThickness = 2;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ���������� ��������
        /// </summary>
        private const double NotchsHandStartRadius = 0.85;

        /// <summary>
        /// �� ���� ���������� �� ������ ����� ������������� ��������
        /// </summary>
        private const double NotchsHandEndRadius = 0.95;

        /// <summary>
        /// ������� ��������
        /// </summary>
        private const double NotchesThickness = 4;

        /// <summary>
        /// ������ ��������
        /// </summary>
        private int _width;
        
        /// <summary>
        /// ������ ��������
        /// </summary>
        private int _height;

        /// <summary>
        /// ����������� ����� �������� (���������� - ����� �����)
        /// </summary>
        private Point _centerPoint = new Point(0, 0);

        /// <summary>
        /// ����������� �� ������ ��������
        /// </summary>
        private int _minSide;

        /// <summary>
        /// ������ �����
        /// </summary>
        private double _clockRadius;

        #region �������� ��������

        #region �����

        /// <summary>
        /// �����-�� �����, ������� ����������� ��������, ������� ����� �������(������� ��� ��� ������ ����)
        /// </summary>
        public static readonly AttachedProperty<DateTime> TimeProperty
            = AvaloniaProperty.RegisterAttached<Clock, Interactive, DateTime>(nameof(Time));

        /// <summary>
        /// �������� � ��������, ������� �������� � �������
        /// </summary>
        public DateTime Time
        {
            get { return GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        #endregion

        #endregion

        public Clock()
        {
            InitializeComponent();

            // ������������ ���������� ��������� ������� ��������
            PropertyChanged += OnPropertyChangedListener;

            // ����� �������� �������� �������� Time, ������� ����� HandleTimeChanged()
            TimeProperty.Changed.Subscribe(x => HandleTimeChanged(x.Sender, x.NewValue.GetValueOrDefault<DateTime>()));
        }

        /// <summary>
        /// ����� ����������, ����� �������� �����-�� �������� ��������
        /// </summary>
        private void OnPropertyChangedListener(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name.Equals("Bounds")) // ���� �������� �������� � ������ Bounds (������� ��������)
            {
                OnResize((Rect)e.NewValue); // �������� OnResize, ������� ���� ����� ������� ��������
            }
        }

        /// <summary>
        /// ����� ���������� ��� ��������� �������� ��������
        /// </summary>
        private void OnResize(Rect bounds)
        {
            _width = (int)bounds.Width;
            _height = (int)bounds.Height;

            _minSide = Math.Min(_width, _height);

            _clockRadius = _minSide / 2.0;

            _centerPoint = new Point(_width / 2.0, _height / 2.0); // ����� �� 2.0, � �� �� 2, ����� ���� ������ �������
        }

        /// <summary>
        /// ����� ��������� ����������� ��������
        /// </summary>
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // ������ ����� �������� ������ ����

            // ���������� ����������
            context.DrawEllipse
            (
                new SolidColorBrush(Colors.Transparent),
                new Pen(new SolidColorBrush(Colors.Black)),
                _centerPoint,
                _minSide / 2.0,
                _minSide / 2.0
            );

            DrawNotches(context);

            var secondsFromDayBegin = Time.TimeOfDay.TotalSeconds;

            double hoursFraction = secondsFromDayBegin / 3600.0;
            double minutesFraction = (secondsFromDayBegin % 3600.0) / 60.0;
            double secondsFraction = secondsFromDayBegin % 60.0;

            // ���������� �������
            DrawHoursHand(context, hoursFraction);
            DrawMinutesHand(context, minutesFraction);
            DrawSecondsHand(context, secondsFraction);
        }

        /// <summary>
        /// ���� ����� ����� ����������, ����� �������� ������������ �����
        /// </summary>
        private void HandleTimeChanged(AvaloniaObject sender, DateTime dateTime)
        {
            InvalidateVisual(); // ���� ����� ���������� ������� ������������ ����
        }

        /// <summary>
        /// ��������� ���������� ������ � ����� ������� (��� �������� �� ����������)
        /// </summary>
        /// <param name="r1">���������� ������ ������� �� ������ �����</param>
        /// <param name="r2">���������� ����� ������� �� ������ �����</param>
        /// <param name="t">��������� ������� [0; 2*Pi]</param>
        /// <returns>����� � ����� ������� - ������ ����� - ������, ������ ����� - �����</returns>
        private Tuple<Point, Point> GetHandCoordinates(double r1, double r2, double t)
        {
            if (r1 < 0)
            {
                throw new ArgumentOutOfRangeException("R1 �� ����� ���� �������������!", nameof(r1));
            }

            if (r2 < r1)
            {
                throw new ArgumentOutOfRangeException("R2 ������ ���� ������ ��� ����� R1!", nameof(r2));
            }

            if (t < 0 || t > 2 * Math.PI)
            {
                throw new ArgumentOutOfRangeException("T ������ ���� �� 0 �� 2PI!", nameof(t));
            }

            double sint = Math.Sin(t);
            double cost = -1 * Math.Cos(t);

            Point start = new Point(r1 * sint + _centerPoint.X, r1 * cost + _centerPoint.Y);
            Point end = new Point(r2 * sint + _centerPoint.X, r2 * cost + _centerPoint.Y);

            return new Tuple<Point, Point>(start, end);
        }

        /// <summary>
        /// ��� ������� �������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hours"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void DrawHoursHand(DrawingContext context, double hours)
        {
            if (hours < 0 || hours > 24)
            {
                throw new ArgumentOutOfRangeException("����� ����� ������ ���� �� 0 �� 24!", nameof(hours));
            }

            double normalizedHours = hours;
            if (normalizedHours > 12)
            {
                normalizedHours -= 12;
            }

            double t = (normalizedHours / 12.0) * 2 * Math.PI;

            var handCoords = GetHandCoordinates(_clockRadius * HoursHandStartRadius, _clockRadius * HoursHandEndRadius, t);

            context.DrawLine
            (
                new Pen(new SolidColorBrush(Colors.Black), HoursArrowThickness),
                handCoords.Item1,
                handCoords.Item2
            );
        }

        /// <summary>
        /// ��� �������� �������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="minutes"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void DrawMinutesHand(DrawingContext context, double minutes)
        {
            if (minutes < 0 || minutes > 60)
            {
                throw new ArgumentOutOfRangeException("����� ����� ������ ���� �� 0 �� 60!", nameof(minutes));
            }

            double t = (minutes / 60.0) * 2 * Math.PI;

            var handCoords = GetHandCoordinates(_clockRadius * MinutesHandStartRadius, _clockRadius * MinutesHandEndRadius, t);

            context.DrawLine
            (
                new Pen(new SolidColorBrush(Colors.Black), MinutesArrowThickness),
                handCoords.Item1,
                handCoords.Item2
            );
        }

        /// <summary>
        /// ��� ��������� �������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="minutes"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void DrawSecondsHand(DrawingContext context, double seconds)
        {
            if (seconds < 0 || seconds > 60)
            {
                throw new ArgumentOutOfRangeException("����� ������ ������ ���� �� 0 �� 60!", nameof(seconds));
            }

            double t = (seconds / 60.0) * 2 * Math.PI;

            var handCoords = GetHandCoordinates(_clockRadius * SecondsHandStartRadius, _clockRadius * SecondsHandEndRadius, t);

            context.DrawLine
            (
                new Pen(new SolidColorBrush(Colors.Black), SecondsArrowThickness),
                handCoords.Item1,
                handCoords.Item2
            );
        }

        /// <summary>
        /// ����� ��������� ����� ��� ������ �������� �� ���������
        /// </summary>
        private void DrawNotches(DrawingContext context)
        {
            for (int notchNumber = 1; notchNumber <= 12; notchNumber++)
            {
                DrawNotch(context, notchNumber);
            }
        }

        /// <summary>
        /// ���� ����� ������ ���� ��������
        /// </summary>
        private void DrawNotch(DrawingContext context, int notchNumber)
        {
            var angle = (notchNumber / 12.0) * 2 * Math.PI;

            var notchCoords = GetHandCoordinates(_clockRadius * NotchsHandStartRadius, _clockRadius * NotchsHandEndRadius, angle);

            context.DrawLine
            (
                new Pen(new SolidColorBrush(Colors.Black), NotchesThickness),
                notchCoords.Item1,
                notchCoords.Item2
            );

            context.DrawText
            (
                new FormattedText
                (
                    $"{notchNumber}",
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface
                    (
                        FontFamily.Default,
                        FontStyle.Italic,
                        FontWeight.Light,
                        FontStretch.Condensed
                     ),
                    NotchTextSize,
                    new SolidColorBrush(NotchTextColor)
                ),
                notchCoords.Item1
            );
        }

        private void DrawText(DrawingContext context, int notchNumber)
        {
            var angle = (notchNumber / 12.0) * 2 * Math.PI;
        }


    }
}
