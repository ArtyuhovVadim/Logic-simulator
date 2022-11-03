using System.Collections.ObjectModel;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class SchemeViewModel : BindableBase
{
    private readonly Scheme _scheme;

    private readonly SelectionTool _selectionTool = new();
    private readonly RectangleSelectionTool _rectangleSelectionTool = new();
    private readonly DragTool _dragTool = new();
    private readonly NodeDragTool _nodeDragTool = new();

    private readonly IEditorSelectionService _editorSelectionService;

    public SchemeViewModel(Scheme scheme)
    {
        _scheme = scheme;

        _tools = new ObservableCollection<BaseTool> { _selectionTool, _rectangleSelectionTool, _dragTool, _nodeDragTool };

        Objects = new ObservableCollection<BaseSceneObject>(_scheme.Objects);

        _selectionTool.SelectionChanged += OnSelectionChanged;
        _rectangleSelectionTool.SelectionChanged += OnSelectionChanged;

        _editorSelectionService = App.Host.Services.GetRequiredService<IEditorSelectionService>();
    }

    private void OnSelectionChanged()
    {
        _editorSelectionService.Select(this);
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
            //A4 - 210×297
            Width = 2970,
            Height = 2100,
            CellSize = 25,
            Background = new Color4(1, 0.9882353f, 0.972549f, 1f),
            LineColor = new Color4(0.9411765f, 0.9411765f, 0.9215686f, 1f),
            BoldLineColor = new Color4(0.8627451f, 0.8627451f, 0.8431373f, 1f),
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

    private ObservableCollection<BaseTool> _tools;
    public ObservableCollection<BaseTool> Tools
    {
        get => _tools;
        set => Set(ref _tools, value);
    }

    #endregion
}