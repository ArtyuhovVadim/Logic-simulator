using LogicSimulator.Models.Common;
using LogicSimulator.Models.Logic;
using LogicSimulator.ViewModels.Logic.Gates.Base;
using LogicSimulator.ViewModels.Objects.Base;

namespace LogicSimulator.ViewModels.Logic;

public class PortViewModel : BaseObjectViewModel, IModelBased<PortModel>
{
    public PortViewModel(PortModel model, BaseGateViewModel parent)
    {
        Model = model;
        Parent = parent;
    }

    public override PortModel Model { get; }

    public BaseGateViewModel Parent { get; }

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

    public override PortViewModel MakeClone() => throw new NotSupportedException();
}