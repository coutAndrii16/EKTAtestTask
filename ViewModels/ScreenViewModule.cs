using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EKTAtestTask.Models;
using EKTAtestTask.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Threading;

namespace EKTAtestTask.ViewModels;


public partial class ScreenViewModel : BaseViewModel
{
    // Параметри екрану

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(1, 20, ErrorMessage = "Від 1 до 20")]
    [NotifyCanExecuteChangedFor(nameof(BuildScreenCommand))]
    private int _cabinetsX = 4;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(1, 20, ErrorMessage = "Від 1 до 20")]
    [NotifyCanExecuteChangedFor(nameof(BuildScreenCommand))]
    private int _cabinetsY = 3;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(40, 400, ErrorMessage = "Від 40 до 400 пікс.")]
    [NotifyCanExecuteChangedFor(nameof(BuildScreenCommand))]
    private int _cabinetPixelWidth = 120;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(40, 400, ErrorMessage = "Від 40 до 400 пікс.")]
    [NotifyCanExecuteChangedFor(nameof(BuildScreenCommand))]
    private int _cabinetPixelHeight = 120;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(1, 16, ErrorMessage = "Від 1 до 16")]
    [NotifyCanExecuteChangedFor(nameof(BuildScreenCommand))]
    private int _modulesX = 4;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(1, 16, ErrorMessage = "Від 1 до 16")]
    [NotifyCanExecuteChangedFor(nameof(BuildScreenCommand))]
    private int _modulesY = 4;

    //Стан

    [ObservableProperty]
    private ObservableCollection<CabinetViewModel> _cabinets = [];

    [ObservableProperty]
    private ModuleViewModel? _selectedModule;

    [ObservableProperty]
    private int _screenColumns = 4;

    [ObservableProperty]
    private bool _isTimerRunning;

    [ObservableProperty]
    private string _statusMessage = "Натисніть «Побудувати» для старту.";

    private List<ModuleViewModel> _allModules = [];
    private readonly DispatcherTimer _timer;

    // Конструктор

    public ScreenViewModel()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
        _timer.Tick += OnTimerTick;

        BuildScreen(); // показати стартовий екран одразу
    }

    // Команди 

    [RelayCommand(CanExecute = nameof(CanBuild))]
    private void BuildScreen()
    {
        try
        {
            var config = new ScreenModel
            {
                CabinetsX = CabinetsX,
                CabinetsY = CabinetsY,
                CabinetPixelWidth = CabinetPixelWidth,
                CabinetPixelHeight = CabinetPixelHeight,
                ModulesX = ModulesX,
                ModulesY = ModulesY,
            };

            var cabinets = new List<CabinetViewModel>();
            _allModules = [];

            for (int row = 0; row < config.CabinetsY; row++)
            {
                for (int col = 0; col < config.CabinetsX; col++)
                {
                    var modules = new List<ModuleModel>();
                    for (int mr = 0; mr < config.ModulesY; mr++)
                        for (int mc = 0; mc < config.ModulesX; mc++)
                            modules.Add(new ModuleModel
                            {
                                CabinetRow = row, CabinetCol = col,
                                ModuleRow = mr,   ModuleCol = mc,
                                Temperature = TemperatureService.Generate()
                            });

                    var cab = new CabinetViewModel(new CabinetModel
                    {
                        Row = row, Col = col, Modules = modules
                    }, config);
                    cabinets.Add(cab);
                    _allModules.AddRange(cab.Modules);
                }
            }

            Cabinets = new ObservableCollection<CabinetViewModel>(cabinets);
            ScreenColumns = config.CabinetsX;
            SelectedModule = null;
            StatusMessage = $"Екран {config.CabinetsX}×{config.CabinetsY} кабінетів, " +
                            $"{config.ModulesX}×{config.ModulesY} модулів кожен.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Помилка побудови: {ex.Message}";
        }
    }

    private bool CanBuild() => !HasErrors;

    [RelayCommand]
    private void SelectModule(ModuleViewModel? module) =>
        SelectedModule = module;

    [RelayCommand]
    private void ToggleTimer()
    {
        if (_timer.IsEnabled)
        {
            _timer.Stop();
            IsTimerRunning = false;
            StatusMessage = "Таймер зупинено.";
        }
        else
        {
            if (_allModules.Count == 0)
            {
                StatusMessage = "Спочатку побудуйте екран.";
                return;
            }
            _timer.Start();
            IsTimerRunning = true;
            StatusMessage = "Таймер запущено (оновлення кожні 2 сек).";
        }
    }

    // Таймер 

    private void OnTimerTick(object? sender, EventArgs e)
    {
        try
        {
            foreach (var mv in _allModules)
            {
                mv.Model.Temperature = TemperatureService.Generate();
                mv.RefreshTemperature();
            }

            if (SelectedModule is not null)
                OnPropertyChanged(nameof(SelectedModule)); // оновити панель деталей
        }
        catch (Exception ex)
        {
            _timer.Stop();
            IsTimerRunning = false;
            StatusMessage = $"Таймер зупинено через помилку: {ex.Message}";
        }
    }
}