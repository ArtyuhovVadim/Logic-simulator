using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.ViewModels.Logic.Gates.Base;

namespace LogicSimulator.ViewModels.Logic.Gates;

public class AndGateViewModel : SimpleGateViewModel
{
    public AndGateViewModel() : this(new AndGateModel()) { }

    public AndGateViewModel(AndGateModel model) : base(model)
    {
        Model = model;

        Width = 80;
        Height = 80;
    }

    public override AndGateModel Model { get; }

    public override void AcceptSchemeBuilder(SchemeBuilderService builder) => builder.CreateLogicModelFrom(this);

    public override AndGateViewModel MakeClone() => new(Model.MakeClone());
}