using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.Tools;

public class RectanglePlacingToolViewModel : BasePlacingToolViewModel<RectangleViewModel>
{
    public RectanglePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme) { }

    protected override PlacingStep<RectangleViewModel> ConfigurePlacingSteps(PlacingStepsBuilder<RectangleViewModel> builder) => builder
        .AddStep(SetLocation)
            .UseGrid()
            .Then()
        .AddStep(SetSize)
            .UseGrid()
            .Then()
        .Build();

    private static void SetLocation(RectangleViewModel obj, Vector2 pos)
    {
        obj.Location = pos;
        obj.Width = 100;
        obj.Height = 100;
    }

    private static void SetSize(RectangleViewModel obj, Vector2 pos)
    {
        obj.Width = pos.X - obj.Location.X;
        obj.Height = pos.Y - obj.Location.Y;
    }
}