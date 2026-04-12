namespace EKTAtestTask.Models;

public class CabinetModel
{
    public int Row { get; init; }
    public int Col { get; init; }
    public List<ModuleModel> Modules { get; init; } = [];
}