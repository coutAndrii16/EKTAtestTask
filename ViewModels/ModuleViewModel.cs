using CommunityToolkit.Mvvm.ComponentModel;
using EKTAtestTask.Models;
using System.Windows.Media;

namespace EKTAtestTask.ViewModels;

public partial class ModuleViewModel : ObservableObject
{
    public ModuleModel Model { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundColor))]
    private double _temperature;

    public ModuleViewModel(ModuleModel model)
    {
        Model = model;
        _temperature = model.Temperature;
    }

    public void RefreshTemperature()
    {
        Temperature = Model.Temperature;
    }

    public SolidColorBrush BackgroundColor => Temperature switch
    {
        < 30  => new SolidColorBrush(Color.FromRgb(70, 130, 230)),   // синій
        < 55  => new SolidColorBrush(Color.FromRgb(60, 200, 100)),   // зелений
        < 75  => new SolidColorBrush(Color.FromRgb(255, 180, 40)),   // жовтий
        _     => new SolidColorBrush(Color.FromRgb(220, 50, 50)),    // червоний
    };

    public string TooltipText =>
        $"{Model.DisplayId}\nТемпература: {Temperature:F1} °C";
}