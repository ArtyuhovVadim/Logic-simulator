using System.Collections.Generic;
using System.Windows;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

public class RowLayoutBuilder
{
    private readonly List<GridLength> _layout;

    public RowLayoutBuilder() => _layout = new List<GridLength>();

    public RowLayoutBuilder WithAutoSize()
    {
        _layout.Add(GridLength.Auto);
        return this;
    }

    public RowLayoutBuilder WithFixedSize(double value)
    {
        _layout.Add(new GridLength(value, GridUnitType.Pixel));
        return this;
    }

    public RowLayoutBuilder WithRelativeSize(double value)
    {
        _layout.Add(new GridLength(value, GridUnitType.Star));
        return this;
    }

    public List<GridLength> Build() => _layout;
}