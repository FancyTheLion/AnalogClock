using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Globalization;

namespace ThermometrApp.Controls
{
    public partial class ThermomtrControl : UserControl
    {
        /// <summary>
        /// ������
        /// </summary>
        private const double Radius = 15;

        /// <summary>
        /// ���������� ������ ����������
        /// </summary>
        private const double PipeHalfWidth = 5;

        /// <summary>
        /// ���������� ��������
        /// </summary>
        private const double NotchHalfWidth = 30;

        #region �������� ��������

        #region Min t

        /// <summary>
        /// �����-�� �����, ������� ����������� ��������, ������� ����� �������(������� ��� ��� ������ ����)
        /// </summary>
        public static readonly AttachedProperty<double> MinTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(MinTemperature));

        /// <summary>
        /// �������� � ����������� ������������, ������� �������� � �������
        /// </summary>
        public double MinTemperature
        {
            get { return GetValue(MinTemperatureProperty); }
            set { SetValue(MinTemperatureProperty, value); }
        }

        #endregion

        #region Max t

        /// <summary>
        /// �����-�� �����, ������� ����������� ��������, ������� ����� �������(������� ��� ��� ������ ����)
        /// </summary>
        public static readonly AttachedProperty<double> MaxTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(MaxTemperature));

        /// <summary>
        /// �������� � ������������ ������������, ������� �������� � �������
        /// </summary>
        public double MaxTemperature
        {
            get { return GetValue(MaxTemperatureProperty); }
            set { SetValue(MaxTemperatureProperty, value); }
        }

        #endregion

        #region Current t

        /// <summary>
        /// �����-�� �����, ������� ����������� ��������, ������� ����� �������(������� ��� ��� ������ ����)
        /// </summary>
        public static readonly AttachedProperty<double> CurrentTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(CurrentTemperature));

        /// <summary>
        /// �������� � ������� ������������, ������� �������� � �������
        /// </summary>
        public double CurrentTemperature
        {
            get { return GetValue(CurrentTemperatureProperty); }
            set { SetValue(CurrentTemperatureProperty, value); }
        }

        #endregion

        #endregion

        #region ������ "������"

        /// <summary>
        /// ������ ��������
        /// </summary>
        private int _width;

        /// <summary>
        /// ������ ��������
        /// </summary>
        private int _height;

        #endregion

        #region ���������� ��������� ��������

        /// <summary>
        /// �������� �������� (�� X)
        /// </summary>
        private double _middleX;

        /// <summary>
        /// ����� - ����� ����������
        /// </summary>
        private Point _circleCenter = new Point(0, 0);

        /// <summary>
        /// ��� (�� Y) "�������" � ���������
        /// </summary>
        private double _rulerBottomY;

        /// <summary>
        /// ����� ���������� ������� ������ ����������
        /// </summary>
        private double _pipeLeft;

        /// <summary>
        /// ������ ���������� ������ ����������
        /// </summary>
        private double _pipeRight;

        /// <summary>
        /// ������� ������� ������� ��������
        /// </summary>
        private int _liquidTopY;

        /// <summary>
        /// ����� X-���������� ��������
        /// </summary>
        private double _notchLeft;

        /// <summary>
        /// ������ X-���������� ��������
        /// </summary>
        private double _notchRight;


        #endregion

        public ThermomtrControl()
        {
            InitializeComponent();

            // ������������ ���������� ��������� ������� ��������
            PropertyChanged += OnPropertyChangedListener;

            // ����� �������� �������� �������� MinTemperature, ������� ����� HandlMinTemperatureChanged()
            MinTemperatureProperty.Changed.Subscribe(x => HandlMinTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));

            // ����� �������� �������� �������� MaxTemperature, ������� ����� HandlMaxTemperatureChanged()
            MaxTemperatureProperty.Changed.Subscribe(x => HandlMaxTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));

            // ����� �������� �������� �������� CurrentTemperature, ������� ����� HandlCurrentTemperatureChanged()
            CurrentTemperatureProperty.Changed.Subscribe(x => HandlCurrentTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));
        }

        /// <summary>
        /// ���� ����� ����� ����������, ����� �������� ����������� �����������
        /// </summary>
        private void HandlMinTemperatureChanged(AvaloniaObject sender, double minTemperature)
        {
            CalculateCurrentTemperatureLiquidLevel();
            InvalidateVisual(); // ���� ����� ���������� ������� ������������ ����
        }

        /// <summary>
        /// ���� ����� ����� ����������, ����� �������� ������������ �����������
        /// </summary>
        private void HandlMaxTemperatureChanged(AvaloniaObject sender, double maxTemperature)
        {
            CalculateCurrentTemperatureLiquidLevel();
            InvalidateVisual(); // ���� ����� ���������� ������� ������������ ����
        }

        /// <summary>
        /// ���� ����� ����� ����������, ����� �������� ������� �����������
        /// </summary>
        private void HandlCurrentTemperatureChanged(AvaloniaObject sender, double currentTemperature)
        {
            CalculateCurrentTemperatureLiquidLevel();
            InvalidateVisual(); // ���� ����� ���������� ������� ������������ ����
        }

        /// <summary>
        /// ����� ���������� ��� ��������� �������� ��������
        /// </summary>
        private void OnResize(Rect bounds)
        {
            _width = (int)bounds.Width;
            _height = (int)bounds.Height;

            _middleX = _width / 2.0;

            _circleCenter = new Point(_middleX, _height - Radius);

            _rulerBottomY = _height - 2 * Radius;

            _pipeLeft = _middleX - PipeHalfWidth;
            _pipeRight = _middleX + PipeHalfWidth;

            _notchLeft = _middleX - NotchHalfWidth;
            _notchRight = _middleX + NotchHalfWidth;

            CalculateCurrentTemperatureLiquidLevel();
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
        /// ����� ��������� ����������� ��������
        /// </summary>
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // ������ ����� �������� ������ ����

            // ������ �����\�������������
            context.DrawRectangle
            (
                new Pen(new SolidColorBrush(Colors.Black)),
                new Rect(0, 0, _width, _height)
            );

            // ����� ����
            context.DrawEllipse
            (
                new SolidColorBrush(Colors.Red),
                new Pen(new SolidColorBrush(Colors.Black)),
                _circleCenter,
                Radius,
                Radius
            );

            // ������ ��������
            for (double notchTemperature = MinTemperature; notchTemperature <= MaxTemperature; notchTemperature ++)
            {
                DrawNotch(context, notchTemperature);
            }

            // ����� ���������� ����� ����������� (� ����������� �� 3 ������� �����������)
            context.DrawRectangle
            (
                new SolidColorBrush(Colors.Red),
                new Pen(new SolidColorBrush(Colors.Red)),
                new Rect(new Point(_pipeLeft, _liquidTopY), new Point(_pipeRight, _rulerBottomY))
            );

            // ������ ������ ������
            context.DrawRectangle
            (
                new Pen(new SolidColorBrush(Colors.Black)),
                new Rect(new Point(_pipeLeft, 0), new Point(_pipeRight, _rulerBottomY))
            );
        }

        private void CalculateCurrentTemperatureLiquidLevel()
        {
            _liquidTopY = CalculateLiquidTopLevelY(CurrentTemperature);
        }

        private int CalculateLiquidTopLevelY(double temperature)
        {
            double a = _rulerBottomY / (MinTemperature - MaxTemperature);
            double b = -1 * _rulerBottomY * MaxTemperature / (MinTemperature - MaxTemperature);

            return (int)Math.Floor(a * temperature + b + 0.5);
        }

        private void DrawNotch(DrawingContext context, double notchTemperature)
        {

            int topLevelY = CalculateLiquidTopLevelY(notchTemperature);

            Point rightNotchPoint = new Point(_notchRight, topLevelY);

            context.DrawLine
            (
                new Pen(new SolidColorBrush(Colors.Black), 1),
                new Point(_notchLeft, topLevelY),
                rightNotchPoint
            );

            context.DrawText
            (
                new FormattedText
                (
                    $"{notchTemperature}",
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface
                    (
                        FontFamily.Default,
                        FontStyle.Italic,
                        FontWeight.Light,
                        FontStretch.Condensed
                        ),
                    19,
                    new SolidColorBrush(Colors.Black)
                ),
                rightNotchPoint
            );
        }
    }
}
