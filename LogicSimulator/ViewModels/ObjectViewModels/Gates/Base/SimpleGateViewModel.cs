using LogicSimulator.Core;
using LogicSimulator.Models.Base;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;

public abstract class SimpleGateViewModel : BaseGateViewModel
{
    protected SimpleGateViewModel(SimpleGateModel model) : base(model) => Model = model;

    public override SimpleGateModel Model { get; }

    #region Delay

    public ulong Delay
    {
        get => Model.LogicModel.Delay;
        set => Set(Model.LogicModel.Delay, value, Model.LogicModel, (model, value) => model.Delay = value);
    }

    #endregion

    #region InputPortsCount

    public int InputPortsCount
    {
        get => Model.LogicModel.InputPortsCount;
        set => Set(Model.LogicModel.InputPortsCount, value, Model.LogicModel, (model, value) => model.InputPortsCount = value);
    }

    #endregion

    #region OutputState

    public SignalType OutputState => Model.LogicModel.Output.State;

    #endregion

    #region InputStates

    public IEnumerable<SignalType> InputStates => Model.LogicModel.Inputs.Select(x => x.State);

    #endregion

    public virtual void Invalidate()
    {
        OnPropertyChanged(nameof(OutputState));
        OnPropertyChanged(nameof(InputStates));
    }
}