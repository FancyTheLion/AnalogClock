using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace ThermometrApp.Controls
{
    public partial class ThermomtrControl : UserControl
    {
        #region Ѕиндимые свойства

        #region Min t

        /// <summary>
        ///  ака€-то хрень, кажетс€ регистраци€ свойства, которое можно биндить(заучить что так должно быть)
        /// </summary>
        public static readonly AttachedProperty<double> MinTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(MinTemperature));

        /// <summary>
        /// —войство с минимальной температурой, которое биндитс€ в контрол
        /// </summary>
        public double MinTemperature
        {
            get { return GetValue(MinTemperatureProperty); }
            set { SetValue(MinTemperatureProperty, value); }
        }

        #endregion

        #region Max t

        /// <summary>
        ///  ака€-то хрень, кажетс€ регистраци€ свойства, которое можно биндить(заучить что так должно быть)
        /// </summary>
        public static readonly AttachedProperty<double> MaxTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(MaxTemperature));

        /// <summary>
        /// —войство с максимальной температурой, которое биндитс€ в контрол
        /// </summary>
        public double MaxTemperature
        {
            get { return GetValue(MaxTemperatureProperty); }
            set { SetValue(MaxTemperatureProperty, value); }
        }

        #endregion

        #region Current t

        /// <summary>
        ///  ака€-то хрень, кажетс€ регистраци€ свойства, которое можно биндить(заучить что так должно быть)
        /// </summary>
        public static readonly AttachedProperty<double> CurrentTemperatureProperty
            = AvaloniaProperty.RegisterAttached<ThermomtrControl, Interactive, double>(nameof(CurrentTemperature));

        /// <summary>
        /// —войство с текущей температурой, которое биндитс€ в контрол
        /// </summary>
        public double CurrentTemperature
        {
            get { return GetValue(CurrentTemperatureProperty); }
            set { SetValue(CurrentTemperatureProperty, value); }
        }

        #endregion

        #endregion

        public ThermomtrControl()
        {
            InitializeComponent();

            //  огда мен€етс€ значение свойства MinTemperature, вызвать метод HandlMinTemperatureChanged()
            MinTemperatureProperty.Changed.Subscribe(x => HandlMinTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));

            //  огда мен€етс€ значение свойства MaxTemperature, вызвать метод HandlMaxTemperatureChanged()
            MaxTemperatureProperty.Changed.Subscribe(x => HandlMaxTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));

            //  огда мен€етс€ значение свойства CurrentTemperature, вызвать метод HandlCurrentTemperatureChanged()
            CurrentTemperatureProperty.Changed.Subscribe(x => HandlCurrentTemperatureChanged(x.Sender, x.NewValue.GetValueOrDefault<double>()));
        }

        /// <summary>
        /// Ётот метод будет вызыватьс€, когда мен€етс€ минимальна€ температура
        /// </summary>
        private void HandlMinTemperatureChanged(AvaloniaObject sender, double minTemperature)
        {
            InvalidateVisual(); // Ётот метод заставл€ет контрол перерисовать себ€
        }

        /// <summary>
        /// Ётот метод будет вызыватьс€, когда мен€етс€ максимальна€ температура
        /// </summary>
        private void HandlMaxTemperatureChanged(AvaloniaObject sender, double maxTemperature)
        {
            InvalidateVisual(); // Ётот метод заставл€ет контрол перерисовать себ€
        }

        /// <summary>
        /// Ётот метод будет вызыватьс€, когда мен€етс€ текуща€ температура
        /// </summary>
        private void HandlCurrentTemperatureChanged(AvaloniaObject sender, double currentTemperature)
        {
            InvalidateVisual(); // Ётот метод заставл€ет контрол перерисовать себ€
        }


    }
}
