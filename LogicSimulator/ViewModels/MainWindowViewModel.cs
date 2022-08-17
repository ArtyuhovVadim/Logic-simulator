using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Scene.Components.Base;
using LogicSimulator.Scene.Components;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.Scene.Tools.Base;
using LogicSimulator.Scene.Tools;
using LogicSimulator.ViewModels.Base;
using SharpDX;
using Rectangle = LogicSimulator.Scene.SceneObjects.Rectangle;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    private readonly ObservableCollection<Scheme> _schemes = new();

    public MainWindowViewModel(ISchemeFileService schemeFileService, IUserDialogService userDialogService)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

        for (var i = 0; i < 3; i++)
        {
            _schemeFileService.ReadFromFile("Data/Example.lss", out var scheme);

            scheme.Name += $" {i}";

            _schemes.Add(scheme);
        }

        _schemeViewModels = new ObservableCollection<SchemeViewModel>(_schemes.Select(x => new SchemeViewModel(x)));
    }

    #region SchemeViewModels

    private ObservableCollection<SchemeViewModel> _schemeViewModels;

    public ObservableCollection<SchemeViewModel> SchemeViewModels
    {
        get => _schemeViewModels;
        private set => Set(ref _schemeViewModels, value);
    }

    #endregion

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        //var path = "Data/Example.lss";
        //
        //if (!_schemeFileService.ReadFromFile(path, out _scheme))
        //{
        //    _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл:{path}");
        //    return;
        //}
        //
        //Objects.Clear();
        //
        //foreach (var o in _scheme.Objects)
        //    Objects.Add(o);
    }, _ => true);

    #endregion

    #region SaveExampleCommand

    private ICommand _saveExampleCommand;

    public ICommand SaveExampleCommand => _saveExampleCommand ??= new LambdaCommand(_ =>
    {
        //var path = "Data/Example.lss";
        //
        //if (!_schemeFileService.SaveToFile("Data/Example.lss", _scheme))
        //{
        //    _userDialogService.ShowErrorMessage("Ошибка сохранения файла", $"Не удалось сохранить файл: {path}");
        //}
    }, _ => true);

    #endregion

    #region TestCommand

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {

    }, _ => true);

    #endregion

    #region Components

    private ObservableCollection<BaseRenderingComponent> _components = new()
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

    #region Objects

    private ObservableCollection<BaseSceneObject> _objects = new()
    {
        new Rectangle{Width = 100, Height = 100,Location = new Vector2(100,100)},
        new Rectangle{Width = 100, Height = 100,Location = new Vector2(100,100)},
        new Rectangle{Width = 100, Height = 100,Location = new Vector2(100,100)},
        new Rectangle{Width = 100, Height = 100,Location = new Vector2(100,100)},
        new Rectangle{Width = 100, Height = 100,Location = new Vector2(100,100)}
    };

    public ObservableCollection<BaseSceneObject> Objects
    {
        get => _objects;
        set => Set(ref _objects, value);
    }

    #endregion
}