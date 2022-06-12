using System.Collections.ObjectModel;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    #region Objects

    private ObservableCollection<BaseSceneObject> _objects = new()
    {
        new Rectangle { Location = new Vector2(123, 54), Size = new Vector2(300, 500), StrokeColor = new Color4(0, 0, 0, 1), StrokeWidth = 3 },
        new Rectangle { Location = new Vector2(193, 123), Size = new Vector2(300, 500), StrokeColor = new Color4(0, 1, 0, 1), StrokeWidth = 1 },
        new Rectangle { Location = new Vector2(675, 454), Size = new Vector2(300, 500), StrokeColor = new Color4(1, 0, 0, 1), StrokeWidth = 7 },
    };
    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion
}