using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Scene.SceneObjects;
using LogicSimulator.Scene.SceneObjects.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    private Scheme _scheme;

    public MainWindowViewModel(ISchemeFileService schemeFileService, IUserDialogService userDialogService, PropertiesViewModel propertiesViewModel)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

        AnchorableViewModels.Add(propertiesViewModel);
    }

    #region ActiveContent

    private BindableBase _activeContent;

    public BindableBase ActiveContent
    {
        get => _activeContent;
        set => Set(ref _activeContent, value);
    }

    #endregion

    #region SchemeViewModels

    private ObservableCollection<SchemeViewModel> _schemeViewModels = new();

    public ObservableCollection<SchemeViewModel> SchemeViewModels
    {
        get => _schemeViewModels;
        private set => Set(ref _schemeViewModels, value);
    }

    #endregion

    #region AnchorableViewModels

    private ObservableCollection<BindableBase> _anchorableViewModels = new();

    public ObservableCollection<BindableBase> AnchorableViewModels
    {
        get => _anchorableViewModels;
        private set => Set(ref _anchorableViewModels, value);
    }

    #endregion

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        var path = "Data/Example.lss";

        //if (!_schemeFileService.ReadFromFile(path, out var scheme))
        //{
        //    _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {path}");
        //    return;
        //}
        //
        //for (int i = 0; i < 300; i++)
        //{
        //    var line = new Line { StrokeThickness = 10 };
        //
        //    line.Vertices.Add(new Vertex(500, 500));
        //    line.Vertices.Add(new Vertex(800, 500));
        //    line.Vertices.Add(new Vertex(800, 800));
        //
        //    scheme.Objects.Add(line);
        //}
        //
        //scheme.Objects.Add(new Rectangle());

        _scheme = new Scheme { Name = "Line test" };

        var list = new List<BaseSceneObject>();

        var line = new Line();
        line.AddVertex(new Vector2(300, 300));
        line.AddVertex(new Vector2(500, 300));
        line.AddVertex(new Vector2(500, 500));

        list.Add(line);

        _scheme.Objects = list;

        SchemeViewModels.Add(new SchemeViewModel(_scheme));
    }, _ => true);

    #endregion

    #region SaveExampleCommand

    private ICommand _saveExampleCommand;

    public ICommand SaveExampleCommand => _saveExampleCommand ??= new LambdaCommand(_ =>
    {

    }, _ => true);

    #endregion

    #region TestCommand

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {
        //foreach (var line in SchemeViewModels.First().Objects.OfType<Line>())
        //{
        //    //line.Vertices.Clear();
        //}

        //_scheme.Objects.Clear();
        //
        //SchemeViewModels.First().Objects.Clear();
        //
        //GC.Collect(3, GCCollectionMode.Forced);

        SchemeViewModels.First().Objects.OfType<Line>().First().AddVertex(new Vector2(100,200));
    }, _ => true);

    #endregion
}