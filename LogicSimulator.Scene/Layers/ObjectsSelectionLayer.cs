using System.Windows;
using LogicSimulator.Scene.Layers.Base;
using LogicSimulator.Scene.Views.Base;

namespace LogicSimulator.Scene.Layers;

public class ObjectsSelectionLayer : BaseSceneLayer
{
    #region Views

    public IEnumerable<SceneObjectView> Views
    {
        get => (IEnumerable<SceneObjectView>)GetValue(ViewsProperty);
        set => SetValue(ViewsProperty, value);
    }

    public static readonly DependencyProperty ViewsProperty =
        DependencyProperty.Register(nameof(Views), typeof(IEnumerable<SceneObjectView>), typeof(ObjectsSelectionLayer), new PropertyMetadata(default(IEnumerable<SceneObjectView>), DefaultPropertyChangedHandler));

    #endregion
}