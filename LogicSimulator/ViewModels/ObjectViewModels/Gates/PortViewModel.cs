using LogicSimulator.Core;
using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Gates.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.ObjectViewModels.Gates;

public class PortViewModel : BaseObjectViewModel, IModelBased<PortModel>
{
    public PortViewModel(PortModel model, BaseGateViewModel parent)
    {
        Model = model;
        Parent = parent;
    }

    public override PortModel Model { get; }

    public BaseGateViewModel Parent { get; }

    public Vector2 AbsoluteLocation => Parent.Location + (Location + new Vector2(Length, 0).Transform(Rotation)).Transform(Parent.Rotation);

    #region Name

    public string Name
    {
        get => Model.Name;
        set => Set(Model.Name, value, Model, (model, value) => model.Name = value);
    }

    #endregion

    #region Length

    public float Length
    {
        get => Model.Length;
        set => Set(Model.Length, value, Model, (model, value) => model.Length = value);
    }

    #endregion

    #region State

    private SignalType _state = SignalType.Undefined;

    public SignalType State
    {
        get => _state;
        set => Set(ref _state, value);
    }

    #endregion

    public override PortViewModel MakeClone() => throw new NotSupportedException();
}