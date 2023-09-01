using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace ThermometrApp.Controls
{
    public partial class ThermomtrControl : UserControl
    {
        /// <summary>
        /// Радиус
        /// </summary>
        private const double Radius = 15;

        /// <summary>
        /// Полуширина трубки термометра
        /// </summary>
        private const double PipeHalfWidth = 5;

        #region Биндимые свойства

        #region Min t

        /// <summary>
        /// Какая-то хрень, кажется регистрация свойства, которое можно биндить(заучить что так должно быть)
        /// </summary>
        public static readonly AttachedProperty<double> MinTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(MinTemperature));

        /// <summary>
        /// Свойство с минимальной температурой, которое биндится в контрол
        /// </summary>
        public double MinTemperature
        {
            get { return GetValue(MinTemperatureProperty); }
            set { SetValue(MinTemperatureProperty, value); }
        }

        #endregion

        #region Max t

        /// <summary>
        /// Какая-то хрень, кажется регистрация свойства, которое можно биндить(заучить что так должно быть)
        /// </summary>
        public static readonly AttachedProperty<double> MaxTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(MaxTemperature));

        /// <summary>
        /// Свойство с максимальной температурой, которое биндится в контрол
        /// </summary>
        public double MaxTemperature
        {
            get { return GetValue(MaxTemperatureProperty); }
            set { SetValue(MaxTemperatureProperty, value); }
        }

        #endregion

        #region Current t

        /// <summary>
        /// Какая-то хрень, кажется регистрация свойства, которое можно биндить(заучить что так должно быть)
        /// </summary>
        public static readonly AttachedProperty<double> CurrentTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(CurrentTemperature));

        /// <summary>
        /// Свойство с текущей температурой, которое биндится в контрол
        /// </summary>
        public double CurrentTemperature
        {
            get { return GetValue(CurrentTemperatureProperty); }
            set { SetValue(CurrentTemperatureProperty, value); }
        }

        #endregion

        #endregion

        #region Размер "холста"

        /// <summary>
        /// Ширина контрола
        /// </summary>
        private int _width;

        /// <summary>
        /// Высота контрола
        /// </summary>
        private int _height;

        #endregion

        #region Координаты элементов контрола

        /// <summary>
        /// Середина контрола (по X)
        /// </summary>
        private double _middleX;

        /// <summary>
        /// Точка - центр окружности
        /// </summary>
        private Point _circleCenter = new Point(0, 0);

        /// <summary>
        /// Низ (по Y) "линейки" с делениями
        /// </summary>
        private double _rulerBottomY;

        /// <summary>
        /// Левая верхняя точка шкалы термометра
        /// </summary>
        private double _pipeLeft;

        /// <summary>
        /// Права нижняя точка шкалы термометра
        /// </summary>
        private double _pipeRight;

        #endregion

        public ThermomtrControl()
        {
            InitializeComponent();

            // Регистрируем обработчик изменения свойств контрола
            PropertyChanged += OnPropertyChangedListener;

            // Когда меняется значение свойства MinTemperature, вызвать метод HandlMinTemperatureChanged()
            MinTemperatureProperty.Changed.Subscribe(x => HandlMinTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));

            // Когда меняется значение свойства MaxTemperature, вызвать метод HandlMaxTemperatureChanged()
            MaxTemperatureProperty.Changed.Subscribe(x => HandlMaxTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));

            // Когда меняется значение свойства CurrentTemperature, вызвать метод HandlCurrentTemperatureChanged()
            CurrentTemperatureProperty.Changed.Subscribe(x => HandlCurrentTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));
        }

        /// <summary>
        /// Этот метод будет вызываться, когда меняется минимальная температура
        /// </summary>
        private void HandlMinTemperatureChanged(AvaloniaObject sender, double minTemperature)
        {
            InvalidateVisual(); // Этот метод заставляет контрол перерисовать себя
        }

        /// <summary>
        /// Этот метод будет вызываться, когда меняется максимальная температура
        /// </summary>
        private void HandlMaxTemperatureChanged(AvaloniaObject sender, double maxTemperature)
        {
            InvalidateVisual(); // Этот метод заставляет контрол перерисовать себя
        }

        /// <summary>
        /// Этот метод будет вызываться, когда меняется текущая температура
        /// </summary>
        private void HandlCurrentTemperatureChanged(AvaloniaObject sender, double currentTemperature)
        {
            InvalidateVisual(); // Этот метод заставляет контрол перерисовать себя
        }

        /// <summary>
        /// Метод вызывается при изменении размеров контрола
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
        }

        /// <summary>
        /// Метод вызывается, когда меняются какие-то свойства контрола
        /// </summary>
        private void OnPropertyChangedListener(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name.Equals("Bounds")) // Если меняется свойство с именем Bounds (границы контрола)
            {
                OnResize((Rect)e.NewValue); // Вызываем OnResize, передав туда новые размеры контрола
            }
        }

        /// <summary>
        /// Метод рисования содержимого контрола
        /// </summary>
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // Теперь можно рисовать всякие вещи

            // Рисуем рамку\прямоугольник
            context.DrawRectangle
            (
                new Pen(new SolidColorBrush(Colors.Black)),
                new Rect(0, 0, _width, _height)
            );

            // Рисую круг
            context.DrawEllipse
            (
                new SolidColorBrush(Colors.Red),
                new Pen(new SolidColorBrush(Colors.Black)),
                _circleCenter,
                Radius,
                Radius
            );

            // Рисуем пустую трубку
            context.DrawRectangle
            (
                new Pen(new SolidColorBrush(Colors.Black)),
                new Rect(new Point(_pipeLeft, 0), new Point(_pipeRight, _rulerBottomY))
            );
        }
    }
}
