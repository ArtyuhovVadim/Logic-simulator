using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.ViewModels.Logic.Gates.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Logic.Gates;

public class InputGateViewModel : BaseGateViewModel
{
    public InputGateViewModel() : this(new InputGateModel()) { }

    public InputGateViewModel(InputGateModel model) : base(model)
    {
        Model = model;
        OutputPort = new PortViewModel(model.OutputPort, this);
        Width = 40;
        Height = 30;
    }

    public override InputGateModel Model { get; }

    public override IEnumerable<PortViewModel> Ports => [OutputPort];

    #region OutputPort

    public PortViewModel OutputPort { get; }

    #endregion

    public override InputGateViewModel MakeClone() => new(Model.MakeClone());

    protected override void OnSizeChanged() => OutputPort.Location = new Vector2(Width / 2, 0);
}