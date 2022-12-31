using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Scene;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using SharpDX;
using LogicSimulator.Scene.Tools.PlacingTools;

namespace LogicSimulator.ViewModels;

public class SchemeViewModel : BindableBase
{
    #region Tools

    private readonly TransformTool _transformTool = new();

    private readonly SelectionTool _selectionTool = new();
    private readonly RectangleSelectionTool _rectangleSelectionTool = new();
    private readonly DragTool _dragTool = new() { GridSnap = 25f };
    private readonly NodeDragTool _nodeDragTool = new();

    private readonly RectanglePlacingTool _rectanglePlacingTool = new();
    private readonly EllipsePlacingTool _ellipsePlacingTool = new();
    private readonly TextBlockPlacingTool _textBlockPlacingTool = new();
    private readonly WirePlacingTool _wirePlacingTool = new();

    #endregion

    #region Components

    private readonly GradientClearRenderingComponent _gradientClearRenderingComponent = new()
    {
        StartColor = new Color4(0.755f, 0.755f, 0.755f, 1f),
        EndColor = new Color4(0.887f, 0.887f, 0.887f, 1f)
    };
    private readonly GridRenderingComponent _gridRenderingComponent = new()
    {
        Width = 2970,
        Height = 2100,
        CellSize = 25,
        Background = new Color4(1, 0.9882353f, 0.972549f, 1f),
        LineColor = new Color4(0.9411765f, 0.9411765f, 0.9215686f, 1f),
        BoldLineColor = new Color4(0.8627451f, 0.8627451f, 0.8431373f, 1f),
    };
    private readonly SceneObjectsRenderingComponent _sceneObjectsRenderingComponent = new();
    private readonly SelectionRenderingComponent _selectionRenderingComponent = new();
    private readonly SelectionRectangleRenderingComponent _selectionRectangleRenderingComponent = new();
    private readonly NodeRenderingComponent _nodeRenderingComponent = new()
    {
        BackgroundColor = new Color4(0f, 1f, 0f, 1f)
    };

    #endregion

    private readonly Scheme _scheme;

    private readonly IEditorSelectionService _editorSelectionService;

    public SchemeViewModel(Scheme scheme)
    {
        _scheme = scheme;

        Objects = new ObservableCollection<BaseSceneObject>(_scheme.Objects);

        _components = new ObservableCollection<BaseRenderingComponent>()
        {
            _gradientClearRenderingComponent,
            _gridRenderingComponent,
            _sceneObjectsRenderingComponent,
            _selectionRenderingComponent,
            _selectionRectangleRenderingComponent,
            _nodeRenderingComponent
        };

        var tools = new List<BaseTool>()
        {
            _selectionTool,
            _rectangleSelectionTool,
            _dragTool,
            _nodeDragTool,

            _rectanglePlacingTool,
            _ellipsePlacingTool,
            _textBlockPlacingTool,
            _wirePlacingTool
        };

        ToolsController = new ToolsController(_selectionTool)
        {
            Tools = tools,
            AlwaysUpdatingTools = new[] { _transformTool }
        };

        ToolsController.SelectedObjectsChanged += OnSelectedObjectsChanged;

        _editorSelectionService = App.Host.Services.GetRequiredService<IEditorSelectionService>();
    }

    private void OnSelectedObjectsChanged()
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

    private ObservableCollection<BaseRenderingComponent> _components;
    public ObservableCollection<BaseRenderingComponent> Components
    {
        get => _components;
        set => Set(ref _components, value);
    }

    #endregion

    #region ToolsController

    private ToolsController _toolsController;

    public ToolsController ToolsController
    {
        get => _toolsController;
        set => Set(ref _toolsController, value);
    }

    #endregion
}