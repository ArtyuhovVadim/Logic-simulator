using LogicSimulator.Infrastructure;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

public class OutputGateViewModel : BaseGateViewModel
{
    public OutputGateViewModel(OutputGateModel model) : base(model)
    {
        Model = model;
        InputPort = new PortViewModel(model.InputPort, this);
        Width = 40;
        Height = 30;
    }

    public override OutputGateModel Model { get; }

    #region OutputPort

    public PortViewModel InputPort { get; }

    #endregion

    public override OutputGateViewModel MakeClone() => new(Model.MakeClone());

    protected override void OnSizeChanged()
    {
        InputPort.Rotation = Rotation.Degrees180;
        InputPort.Location = new Vector2(-Width / 2, 0);
    }
}