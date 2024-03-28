using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using LogicSimulator.Models;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

public class AndGateViewModel : SimpleGateViewModel
{
    public AndGateViewModel(AndGateModel model) : base(model) => Model = model;

    public override AndGateModel Model { get; }

    public override AndGateViewModel MakeClone() => new(Model.MakeClone());
}