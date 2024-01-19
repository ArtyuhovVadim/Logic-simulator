using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class ArcPlacingToolViewModel : BasePlacingToolViewModel<ArcViewModel>
{
    private readonly PlacingStep<ArcViewModel> _setSizeStep;
    private readonly PlacingStep<ArcViewModel> _setFirstAngleStep;
    private readonly PlacingStep<ArcViewModel> _setSecondAngleStep;

    public ArcPlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme)
    {
        FirstStep = new PlacingStep<ArcViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _setSizeStep = new PlacingStep<ArcViewModel>(UpdateSize, SizeStepTransition);
        _setFirstAngleStep = new PlacingStep<ArcViewModel>(UpdateFirstAngle, FirstAngleTransition);
        _setSecondAngleStep = new PlacingStep<ArcViewModel>(UpdateSecondAngle, SecondAngleTransition);
    }

    protected override PlacingStep<ArcViewModel> FirstStep { get; }

    private void UpdateLocation(ArcViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Location = pos;
        obj.RadiusX = 100;
        obj.RadiusY = 100;
    }

    private PlacingStep<ArcViewModel> LocationStepTransition(ArcViewModel obj) => _setSizeStep;

    private void UpdateSize(ArcViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.RadiusX = Math.Abs(pos.X - obj.Location.X);
        obj.RadiusY = Math.Abs(pos.Y - obj.Location.Y);
    }

    private PlacingStep<ArcViewModel>? SizeStepTransition(ArcViewModel obj)
    {
        if (obj.RadiusX == 0 || obj.RadiusY == 0)
            return _setSizeStep;

        return _setFirstAngleStep;
    }

    private void UpdateFirstAngle(ArcViewModel obj, Vector2 pos) => obj.StartAngle = MathHelper.GetAngleForArc(obj.Location, obj.RadiusX, pos);

    private PlacingStep<ArcViewModel> FirstAngleTransition(ArcViewModel obj) => _setSecondAngleStep;

    private void UpdateSecondAngle(ArcViewModel obj, Vector2 pos) => obj.EndAngle = MathHelper.GetAngleForArc(obj.Location, obj.RadiusY, pos);

    private PlacingStep<ArcViewModel>? SecondAngleTransition(ArcViewModel obj) => null;
}