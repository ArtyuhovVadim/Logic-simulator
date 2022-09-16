using System.Collections.ObjectModel;
using LogicSimulator.Models;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class SchemeViewModel : BindableBase
{
    private readonly Scheme _scheme;

    public SchemeViewModel(Scheme scheme)
    {
        _scheme = scheme;

        Objects = new ObservableCollection<BaseSceneObject>(_scheme.Objects);
    }

    #region Name

    public string Name
    {
        get => _scheme.Name;
        set
        {
            _scheme.Name = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Scale

    private float _scale = 1f;

    public float Scale
    {
        get => _scale;
        set => Set(ref _scale, value);
    }

    #endregion

    #region MousePosition

    private Vector2 _mousePosition = Vector2.Zero;

    public Vector2 MousePosition
    {
        get => _mousePosition;
        set => Set(ref _mousePosition, value);
    }

    #endregion

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects;

    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion

    #region Components

    private ObservableCollection<BaseRenderingComponent> _components = new()
    {
        new GradientClearRenderingComponent
        {
            StartColor = new Color4(0.755f, 0.755f, 0.755f, 1f),
            EndColor = new Color4(0.887f, 0.887f, 0.887f, 1f)
        },
        new GridRenderingComponent
        {
            Width = 3000,
            Height = 3000,
            CellSize = 25,
            Background = new Color4(1, 252f / 255f, 248f / 255f, 1f),
            LineColor = new Color4(240f / 255f, 240f / 255f, 235f / 255f, 1f),
            BoldLineColor = new Color4(220f / 255f, 220f / 255f, 215f / 255f, 1f),
        },
        new SceneObjectsRenderingComponent(),
        new SelectionRenderingComponent(),
        new SelectionRectangleRenderingComponent(),
        new NodeRenderingComponent { BackgroundColor = new Color4(0f,1f,0f,1f) }
    };
    public ObservableCollection<BaseRenderingComponent> Components
    {
        get => _components;
        set => Set(ref _components, value);
    }

    #endregion

    #region Tools

    private ObservableCollection<BaseTool> _tools = new()
    {
        new SelectionTool(),
        new DragTool(),
        new RectangleSelectionTool(),
        new NodeDragTool()
    };
    public ObservableCollection<BaseTool> Tools
    {
        get => _tools;
        set => Set(ref _tools, value);
    }

    #endregion
}