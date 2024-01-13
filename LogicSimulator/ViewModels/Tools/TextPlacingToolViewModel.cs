using LogicSimulator.Utils;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class TextPlacingToolViewModel : BasePlacingToolViewModel<TextBlockViewModel>
{
    public TextPlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme)
    {
        FirstStep = new PlacingStep<TextBlockViewModel>(UpdateLocation, null, UpdateLocation, LocationStepTransition);
    }

    protected override PlacingStep<TextBlockViewModel> FirstStep { get; }

    private void UpdateLocation(TextBlockViewModel obj, Vector2 pos)
    {
        pos = pos.ApplyGrid(Scheme.GridStep);
        obj.Location = pos;
    }

    private PlacingStep<TextBlockViewModel>? LocationStepTransition(TextBlockViewModel obj) => null;
}