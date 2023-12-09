using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.Models;

public class Scheme
{
    public const string Extension = ".lss";

    public string Name { get; set; } = string.Empty;

    public IEnumerable<BaseObjectViewModel> Objects { get; set; } = Enumerable.Empty<BaseObjectViewModel>();
}