using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.ObjectViewModels;
using LogicSimulator.ViewModels.Tools.Base;

namespace LogicSimulator.ViewModels.Tools;

public class RectanglePlacingToolViewModel : BasePlacingToolViewModel<RectangleViewModel>
{
    public RectanglePlacingToolViewModel(string name, SchemeViewModel scheme) : base(name, scheme)
    {
        AddStep((obj, pos) =>
        {
            obj.Location = pos;
            obj.Width = 100;
            obj.Height = 100;
        });

        AddStep((obj, pos) =>
        {
            obj.Width = pos.X - obj.Location.X;
            obj.Height = pos.Y - obj.Location.Y;
        });
    }
}