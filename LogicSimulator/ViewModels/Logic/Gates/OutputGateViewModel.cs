using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Models.Common;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.ViewModels.Logic.Gates.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Logic.Gates;

public class OutputGateViewModel : BaseGateViewModel
{
    public OutputGateViewModel() : this(new OutputGateModel()) { }

    public OutputGateViewModel(OutputGateModel model) : base(model)
    {
        Model = model;
        InputPort = new PortViewModel(model.InputPort, this);
        Width = 40;
        Height = 30;
    }

    public override OutputGateModel Model { get; }

    public override IEnumerable<PortViewModel> Ports => [InputPort];

    #region InputPort

    public PortViewModel InputPort { get; }

    #endregion

    public override OutputGateViewModel MakeClone() => new(Model.MakeClone());

    public override void AcceptSchemeBuilder(SchemeBuilderService builder) => builder.CreateLogicModelFrom(this);

    protected override void OnSizeChanged()
    {
        InputPort.Rotation = Rotation.Degrees180;
        InputPort.Location = new Vector2(-Width / 2, 0);
    }
}