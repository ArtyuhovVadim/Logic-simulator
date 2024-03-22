using LogicSimulator.Models;
using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class RoundedRectanglePlacingToolViewModel : BasePlacingToolViewModel<RoundedRectangleViewModel>
{
    private readonly PlacingStep<RoundedRectangleViewModel> _setSizeStep;
    private readonly PlacingStep<RoundedRectangleViewModel> _setRadiusStep;

    public RoundedRectanglePlacingToolViewModel(SchemeViewModel scheme) : base(scheme, () => new RoundedRectangleViewModel(new RoundedRectangleModel()))
    {
        FirstStep = new PlacingStep<RoundedRectangleViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _setSizeStep = new PlacingStep<RoundedRectangleViewModel>(UpdateSize, SizeStepTransition);
        _setRadiusStep = new PlacingStep<RoundedRectangleViewModel>(UpdateRadius, RadiusStepTransition);
    }

    protected override PlacingStep<RoundedRectangleViewModel> FirstStep { get; }

    private void UpdateLocation(RoundedRectangleViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Location = pos;
        obj.Width = 100;
        obj.Height = 100;
        obj.RadiusX = 5;
        obj.RadiusY = 5;
    }

    private PlacingStep<RoundedRectangleViewModel> LocationStepTransition(RoundedRectangleViewModel obj) => _setSizeStep;

    private void UpdateSize(RoundedRectangleViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Width = pos.X - obj.Location.X;
        obj.Height = pos.Y - obj.Location.Y;
    }

    private PlacingStep<RoundedRectangleViewModel>? SizeStepTransition(RoundedRectangleViewModel obj)
    {
        if(obj.Width == 0 || obj.Height == 0)
            return _setSizeStep;
        
        return _setRadiusStep;
    }

    private void UpdateRadius(RoundedRectangleViewModel obj, Vector2 pos)
    {
        obj.RadiusX = pos.X - obj.Location.X;
        obj.RadiusY = pos.Y - obj.Location.Y;
    }

    private PlacingStep<RoundedRectangleViewModel>? RadiusStepTransition(RoundedRectangleViewModel obj) => null;
}