using LogicSimulator.ViewModels.StatusViewModels.Base;

namespace LogicSimulator.ViewModels.StatusViewModels;

public class EmptyStatusViewModel : BaseStatusViewModel
{
    public EmptyStatusViewModel() : base(null!) { }
}