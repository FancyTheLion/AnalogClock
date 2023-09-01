using ReactiveUI;
using System;
using System.Reactive;

namespace ThermometrApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Max t
    /// <summary>
    /// Максимальная температура
    /// </summary>
    private double _maxTemperature;

    public double MaxTemperature
    {
        get => _maxTemperature;

        set => this.RaiseAndSetIfChanged(ref _maxTemperature, value);
    }

    #endregion

    #region Min t
    /// <summary>
    /// Минимальная температура
    /// </summary>
    private double _minTemperature;

    public double MinTemperature
    {
        get => _minTemperature;

        set => this.RaiseAndSetIfChanged(ref _minTemperature, value);
    }

    #endregion

    #region Current t
    /// <summary>
    /// Текущая температура
    /// </summary>
    private double _currentTemperature;

    public double CurrentTemperature
    {
        get => _currentTemperature;

        set => this.RaiseAndSetIfChanged(ref _currentTemperature, value);
    }

    #endregion

    #region Команды

    /// <summary>
    /// Команда для увеличения температуры
    /// </summary>
    public ReactiveCommand<Unit, Unit> IncreaseTemperatureCommand { get; set; }

    /// <summary>
    /// Команда для уменьшения температуры
    /// </summary>
    public ReactiveCommand<Unit, Unit> DecreaseTemperatureCommand { get; set; }

    #endregion

    public MainViewModel()
    {
        #region Команды связывания

        IncreaseTemperatureCommand = ReactiveCommand.Create(IncreaseTemperature);
        DecreaseTemperatureCommand = ReactiveCommand.Create(DecreaseTemperature);

        #endregion

        MinTemperature = 30;
        MaxTemperature = 40;
        CurrentTemperature = 35;
    }

    private void IncreaseTemperature()
    {
        if (CurrentTemperature >= MaxTemperature)
        {
            return;
        }

        CurrentTemperature ++;
    }

    private void DecreaseTemperature()
    {
        if (CurrentTemperature <= MinTemperature)
        {
            return;
        }

        CurrentTemperature --;
    }

}
