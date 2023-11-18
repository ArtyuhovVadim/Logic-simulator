using System.Windows;
using LogicSimulator.Scene.Layers.Base;

namespace LogicSimulator.Scene.Layers;

public class ObjectsSelectionLayer : BaseSceneLayer
{
    #region Views

    public IEnumerable<ISelectionRenderable> Views
    {
        get => (IEnumerable<ISelectionRenderable>)GetValue(ViewsProperty);
        set => SetValue(ViewsProperty, value);
    }

    public static readonly DependencyProperty ViewsProperty =
        DependencyProperty.Register(nameof(Views), typeof(IEnumerable<ISelectionRenderable>), typeof(ObjectsSelectionLayer), new PropertyMetadata(default(IEnumerable<ISelectionRenderable>)));

    #endregion
}