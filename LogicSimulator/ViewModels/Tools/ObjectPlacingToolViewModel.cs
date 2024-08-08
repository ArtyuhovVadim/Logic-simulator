using LogicSimulator.Shared.ExtensionMethods;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Objects.Base;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class ObjectPlacingToolViewModel<T> : BasePlacingToolViewModel<T> where T : BaseObjectViewModel, new()
{
    public ObjectPlacingToolViewModel(SchemeViewModel scheme, Func<T> objectFactory) : base(scheme, objectFactory) =>
        FirstStep = new PlacingStep<T>(UpdateLocation, null, UpdateLocation, _ => null);

    public ObjectPlacingToolViewModel(SchemeViewModel scheme) : this(scheme, () => new T()) { }

    protected override PlacingStep<T> FirstStep { get; }

    private void UpdateLocation(T obj, Vector2 pos) => obj.Location = pos.ApplyGrid(Scheme.GridStep);
}