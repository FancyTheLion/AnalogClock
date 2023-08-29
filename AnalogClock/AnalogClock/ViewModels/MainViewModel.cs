using ReactiveUI;
using System;

namespace AnalogClock.ViewModels;

public class MainViewModel : ViewModelBase
{
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
        Time = new DateTime(2023, 08, 29, 13, 37, 50);
    }
}
