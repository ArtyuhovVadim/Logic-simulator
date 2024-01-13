using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class BezierCurvePlacingToolViewModel : BasePlacingToolViewModel<BezierCurveViewModel>
{
    private readonly PlacingStep<BezierCurveViewModel> _setFirstPointStep;
    private readonly PlacingStep<BezierCurveViewModel> _setSecondPointStep;
    private readonly PlacingStep<BezierCurveViewModel> _setThirdPointStep;

    public BezierCurvePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme)
    {
        FirstStep = new PlacingStep<BezierCurveViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _setFirstPointStep = new PlacingStep<BezierCurveViewModel>(UpdateFirstPoint, null, UpdateFirstPoint, SetFirstPointStepTransition);
        _setSecondPointStep = new PlacingStep<BezierCurveViewModel>(UpdateSecondPoint, null, UpdateSecondPoint, SetSecondPointStepTransition);
        _setThirdPointStep = new PlacingStep<BezierCurveViewModel>(UpdateThirdPoint, null, UpdateThirdPoint, SetThirdPointStepTransition);
    }

    protected override PlacingStep<BezierCurveViewModel> FirstStep { get; }

    private void UpdateLocation(BezierCurveViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Location = pos;
        obj.Point1 = Vector2.Zero;
    }

    private PlacingStep<BezierCurveViewModel> LocationStepTransition(BezierCurveViewModel obj) => _setFirstPointStep;

    private void UpdateFirstPoint(BezierCurveViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Point3 = pos - obj.Location;
        obj.Point2 = pos - obj.Location;
    }

    private PlacingStep<BezierCurveViewModel> SetFirstPointStepTransition(BezierCurveViewModel obj) => _setSecondPointStep;

    private void UpdateSecondPoint(BezierCurveViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Point1 = pos - obj.Location;
    }

    private PlacingStep<BezierCurveViewModel> SetSecondPointStepTransition(BezierCurveViewModel obj) => _setThirdPointStep;

    private void UpdateThirdPoint(BezierCurveViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Point2 = pos - obj.Location;
    }

    private PlacingStep<BezierCurveViewModel>? SetThirdPointStepTransition(BezierCurveViewModel obj) => null;
}