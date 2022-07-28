using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel()
    {
        var selectionTool = _tools.OfType<SelectionTool>().First();
        var rectangleSelectionTool = _tools.OfType<RectangleSelectionTool>().First();

        selectionTool.SelectionChanged += OnSelectionChanged;
        rectangleSelectionTool.SelectionChanged += OnSelectionChanged;
    }

    #region SelectedObjects

    private ObservableCollection<BaseSceneObject> _selectedObjects = new();
    public ObservableCollection<BaseSceneObject> SelectedObjects
    {
        get => _selectedObjects;
        private set => Set(ref _selectedObjects, value);
    }

    #endregion

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects = new()
    {
        new Rectangle { Location = new Vector2(100, 100), Width = 200, Height = 300, FillColor = new Color4(1, 0, 0, 1) },
        new Rectangle { Location = new Vector2(250, 250), Width = 200, Height = 300, FillColor = new Color4(0, 0, 1, 1) },
        new Rectangle { Location = new Vector2(300, 400), Width = 200, Height = 300 },
        new Rectangle { Location = new Vector2(350, 250), Width = 200, Height = 300 },
    };
    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion

    #region Components

    private ObservableCollection<BaseRenderingComponent> _renderingComponents = new()
    {
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
        new NodeRenderingComponent()
    };
    public ObservableCollection<BaseRenderingComponent> RenderingComponents
    {
        get => _renderingComponents;
        set => Set(ref _renderingComponents, value);
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

    #region TestCommand

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {
    }, _ => true);

    #endregion

    private void OnSelectionChanged()
    {
        SelectedObjects.Clear();

        foreach (var obj in Objects.Where(x => x.IsSelected))
        {
            SelectedObjects.Add(obj);
        }
    }
}