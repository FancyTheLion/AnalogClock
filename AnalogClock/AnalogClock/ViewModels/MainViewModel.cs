using ReactiveUI;
using System;
using System.Timers;

namespace AnalogClock.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Timer _timer;

    #region Время

    private DateTime _time;


    public DateTime Time
    {
        get => _time;

        set => this.RaiseAndSetIfChanged(ref _time, value);
    }

    #endregion

    public MainViewModel()
    {
        Time = DateTime.Now;

        _timer = new Timer(1000);
        _timer.Elapsed += UpdateTime;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void UpdateTime(Object source, ElapsedEventArgs e)
    {
        Time = DateTime.Now;
    }
}
