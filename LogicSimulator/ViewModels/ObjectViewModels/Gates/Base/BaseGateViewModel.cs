using LogicSimulator.Core.Gates.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;

public abstract class BaseGateViewModel<T> : BaseObjectViewModel where T : BaseGate
{
    protected BaseGateViewModel(T logicModel) => LogicModel = logicModel;

    public T LogicModel { get; }
}