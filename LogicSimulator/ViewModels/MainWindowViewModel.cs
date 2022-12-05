using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using LogicSimulator.Core;
using LogicSimulator.Core.LogicComponents;
using LogicSimulator.Core.LogicComponents.Base;
using LogicSimulator.Core.LogicComponents.Gates;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Scene.SceneObjects.Gates.Base;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;

    public MainWindowViewModel(
        ISchemeFileService schemeFileService,
        IUserDialogService userDialogService,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;

        propertiesViewModel.IsVisible = true;

        AnchorableViewModels.Add(propertiesViewModel);
        AnchorableViewModels.Add(projectExplorerViewModel);

        LoadExampleCommand.Execute(null);
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

    private ObservableCollection<BaseAnchorableViewModel> _anchorableViewModels = new();

    public ObservableCollection<BaseAnchorableViewModel> AnchorableViewModels
    {
        get => _anchorableViewModels;
        private set => Set(ref _anchorableViewModels, value);
    }

    #endregion

    OutputGate outGate1 = new OutputGate();
    OutputGate outGate2 = new OutputGate();

    private Simulator _simulator;

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        //var path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data/Example.lss");
        //
        //if (!_schemeFileService.ReadFromFile(path, out var scheme))
        //{
        //    _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {path}");
        //    return;
        //}

        var scheme = new Scheme { Name = "Test", Objects = new() };

        var inputGate1 = new InputGate();
        var inputGate2 = new InputGate();

        var nor1 = new NorGate(2, 1);
        var nor2 = new NorGate(2, 1);

        var inPort1 = nor1.GetPort(0);
        var inPort2 = nor2.GetPort(1);

        var outPort1 = nor1.GetPort(2);
        var outPort2 = nor2.GetPort(2);

        var wire1 = new Wire(outPort1, nor2.GetPort(0));
        var wire2 = new Wire(outPort2, nor1.GetPort(1));
        
        var outWire1 = new Wire(outGate1.GetPort(0), inPort1);
        var outWire2 = new Wire(outGate2.GetPort(0), inPort2);
        
        new Wire(outPort1, inputGate1.GetPort(0));
        new Wire(outPort2, inputGate2.GetPort(0));

        _simulator = new Simulator(new LogicComponent[]
        {
            outGate1,
            outGate2,
            nor1,
            nor2,
            inputGate1,
            inputGate2,
        });

        scheme.Objects.Add(new NorGateView(nor1) { Location = new Vector2(200, 0) });
        scheme.Objects.Add(new NorGateView(nor2) { Location = new Vector2(200, 200) });

        scheme.Objects.Add(new OutputGateView(outGate1) { Location = new Vector2(0, 0) });
        scheme.Objects.Add(new OutputGateView(outGate2) { Location = new Vector2(0, 200) });
        
        scheme.Objects.Add(new InputGateView(inputGate1) { Location = new Vector2(400, 0) });
        scheme.Objects.Add(new InputGateView(inputGate2) { Location = new Vector2(400, 200) });

        SchemeViewModels.Add(new SchemeViewModel(scheme));
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
        var stopwatch = Stopwatch.StartNew();
        _simulator.Simulate();
        var a = stopwatch.Elapsed;

        _userDialogService.ShowInfoMessage("sssss",$"{a.TotalNanoseconds * 1e-6:0.#####}ms");
    }, _ => true);

    #endregion
}