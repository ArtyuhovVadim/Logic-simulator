using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class RectanglePlacingToolViewModel : BasePlacingToolViewModel<RectangleViewModel>
{
    private readonly PlacingStep<RectangleViewModel> _setSizeStep;

    public RectanglePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme)
    {
        FirstStep = new PlacingStep<RectangleViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
        _setSizeStep = new PlacingStep<RectangleViewModel>(UpdateSize, SizeStepTransition);
    }

    protected override PlacingStep<RectangleViewModel> FirstStep { get; }

    private void UpdateLocation(RectangleViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Location = pos;
        obj.Width = 100;
        obj.Height = 100;
    }

    private PlacingStep<RectangleViewModel> LocationStepTransition(RectangleViewModel obj) => _setSizeStep;

    private void UpdateSize(RectangleViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);

        obj.Width = pos.X - obj.Location.X;
        obj.Height = pos.Y - obj.Location.Y;
    }

    private PlacingStep<RectangleViewModel>? SizeStepTransition(RectangleViewModel obj)
    {
        if(obj.Width == 0 || obj.Height == 0)
            return _setSizeStep;
        
        return null;
    }
}