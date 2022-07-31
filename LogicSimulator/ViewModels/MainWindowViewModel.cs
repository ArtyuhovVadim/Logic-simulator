using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Models;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    private Scheme _scheme;

    public MainWindowViewModel(ISchemeFileService schemeFileService, IUserDialogService userDialogService)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

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

    private ObservableCollection<BaseSceneObject> _objects = new();
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

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        var path = "Data/Example.lss";

        if (!_schemeFileService.ReadFromFile(path, out _scheme))
        {
            _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл:{path}");
            return;
        }

        Objects.Clear();

        foreach (var o in _scheme.Objects)
            Objects.Add(o);
    }, _ => true);

    #endregion

    #region SaveExampleCommand

    private ICommand _saveExampleCommand;

    public ICommand SaveExampleCommand => _saveExampleCommand ??= new LambdaCommand(_ =>
    {
        var path = "Data/Example.lss";

        if (!_schemeFileService.SaveToFile("Data/Example.lss", _scheme))
        {
            _userDialogService.ShowErrorMessage("Ошибка сохранения файла", $"Не удалось сохранить файл: {path}");
        }

    }, _ => true);

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