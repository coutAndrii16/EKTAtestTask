namespace EKTAtestTask.Models;

public class ModuleModel
{
    public int CabinetRow { get; init; }
    public int CabinetCol { get; init; }
    public int ModuleRow { get; init; }
    public int ModuleCol { get; init; }
    public double Temperature { get; set; }
    
    public string DisplayId => $"Каб [{CabinetCol},{CabinetRow}] Мод [{ModuleCol}, {ModuleRow}]";
}