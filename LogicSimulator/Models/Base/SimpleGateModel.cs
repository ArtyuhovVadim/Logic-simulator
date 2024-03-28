using LogicSimulator.Core.Gates.Base;

namespace LogicSimulator.Models.Base;

public abstract class SimpleGateModel : BaseGateModel
{
    protected SimpleGateModel(SimpleGate logicModel) => LogicModel = logicModel;

    public override SimpleGate LogicModel { get; }

    public int InputPortsCount
    {
        get => LogicModel.InputPortsCount;
        set => LogicModel.InputPortsCount = value;
    }

    public ulong Delay
    {
        get => LogicModel.Delay;
        set => LogicModel.Delay = value;
    }
}