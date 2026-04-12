using EKTAtestTask.Models;
using System.Collections.ObjectModel;

namespace EKTAtestTask.ViewModels;


public class CabinetViewModel
{
    public CabinetModel Model { get; }
    public ObservableCollection<ModuleViewModel> Modules { get; }

    public CabinetViewModel(CabinetModel model)
    {
        Model = model;
        Modules = new ObservableCollection<ModuleViewModel>(
            model.Modules.Select(m => new ModuleViewModel(m)));
    }
}