using LogicSimulator.Models;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

public class InputGateViewModel : BaseGateViewModel
{
    public InputGateViewModel(InputGateModel model) : base(model)
    {
        Model = model;
        OutputPort = new PortViewModel(model.OutputPort, this);
        Width = 40;
        Height = 30;
    }

    public override InputGateModel Model { get; }

    #region OutputPort

    public PortViewModel OutputPort { get; }

    #endregion

    public override InputGateViewModel MakeClone() => new(Model.MakeClone());

    protected override void OnSizeChanged() => OutputPort.Location = new Vector2(Width / 2, 0);
}