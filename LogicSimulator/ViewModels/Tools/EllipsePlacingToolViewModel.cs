using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class EllipsePlacingToolViewModel : BasePlacingToolViewModel<EllipseViewModel>
{
    private readonly PlacingStep<EllipseViewModel> _setSizeStep;

    public EllipsePlacingToolViewModel(SchemeViewModel scheme) : base(scheme, () => new EllipseViewModel())
    {
        FirstStep = new PlacingStep<EllipseViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _setSizeStep = new PlacingStep<EllipseViewModel>(UpdateSize, SizeStepTransition);
    }

    protected override PlacingStep<EllipseViewModel> FirstStep { get; }

    private void UpdateLocation(EllipseViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Location = pos;
        obj.RadiusX = 100;
        obj.RadiusY = 50;
    }

    private PlacingStep<EllipseViewModel> LocationStepTransition(EllipseViewModel obj) => _setSizeStep;

    private void UpdateSize(EllipseViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.RadiusX = Math.Abs(pos.X - obj.Location.X);
        obj.RadiusY = Math.Abs(pos.Y - obj.Location.Y);
    }

    private PlacingStep<EllipseViewModel>? SizeStepTransition(EllipseViewModel obj)
    {
        if (obj.RadiusX == 0 || obj.RadiusY == 0)
            return _setSizeStep;

        return null;
    }
}