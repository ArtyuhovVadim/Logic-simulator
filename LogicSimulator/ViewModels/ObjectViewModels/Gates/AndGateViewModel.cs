using LogicSimulator.Infrastructure.Services;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using LogicSimulator.Models;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

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