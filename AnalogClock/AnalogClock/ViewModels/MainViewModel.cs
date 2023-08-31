using ReactiveUI;
using System;
using System.Timers;

namespace AnalogClock.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Timer _timer;

    #region Время (подгоричное)

    private DateTime _timePodgorica;

    public DateTime TimePodgorica
    {
        get => _timePodgorica;

        set => this.RaiseAndSetIfChanged(ref _timePodgorica, value);
    }

    #endregion

    #region Время (московское)

    private DateTime _timeMoscow;

    public DateTime TimeMoscow
    {
        get => _timeMoscow;

        set => this.RaiseAndSetIfChanged(ref _timeMoscow, value);
    }

    #endregion

    public MainViewModel()
    {
        TimePodgorica = GetPodgoricaTime();
        TimeMoscow = GetMoscowTime();

        _timer = new Timer(1000);
        _timer.Elapsed += UpdateTime;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void UpdateTime(Object source, ElapsedEventArgs e)
    {
        TimePodgorica = GetPodgoricaTime();
        TimeMoscow = GetMoscowTime();
    }

    /// <summary>
    /// Метод возвращает подгоричное время
    /// </summary>
    private DateTime GetPodgoricaTime()
    {
        return DateTime.UtcNow.AddHours(2);
    }

    /// <summary>
    /// Метод возвращает московское время
    /// </summary>
    private DateTime GetMoscowTime()
    {
        return DateTime.UtcNow.AddHours(3);
    }
}
