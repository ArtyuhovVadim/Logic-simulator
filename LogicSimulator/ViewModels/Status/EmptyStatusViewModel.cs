using LogicSimulator.ViewModels.Status.Base;

namespace LogicSimulator.ViewModels.Status;

public class EmptyStatusViewModel : BaseStatusViewModel
{
    public EmptyStatusViewModel() : base(null!) { }
}