using System.Windows.Markup;
using AvalonDock.Layout;
using LogicSimulator.ViewModels.AnchorableViewModels;

namespace LogicSimulator.Infrastructure;

public class LayoutInitializer : MarkupExtension, ILayoutUpdateStrategy
{
    private const string LeftPaneName = "LeftPane";
    private const string RightPaneName = "RigthPane";

    public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
    {
        anchorableToShow.AutoHideWidth = 256;
        anchorableToShow.AutoHideHeight = 128;

        return anchorableToShow.Content switch
        {
            ProjectExplorerViewModel => Insert(layout, anchorableToShow, LeftPaneName),
            PropertiesViewModel => Insert(layout, anchorableToShow, RightPaneName),
            _ => false
        };
    }

    public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown) { }

    public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer) => false;

    public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown) { }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    private static bool Insert(LayoutRoot layout, LayoutAnchorable anchorableToShow, string paneName)
    {
        var pane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == paneName);

        if (pane is null)
            return false;

        pane.Children.Add(anchorableToShow);
        return true;
    }
}