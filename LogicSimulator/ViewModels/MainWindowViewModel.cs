using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly ISchemeFileService _schemeFileService;
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;

    public MainWindowViewModel(
        ISchemeFileService schemeFileService,
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel)
    {
        _schemeFileService = schemeFileService;
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;

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

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)!;
        var schemePath = Path.Combine(exePath, "Data/Example.lss");

        if (!_schemeFileService.ReadFromFile(schemePath, out var scheme))
        {
            _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {schemePath}");
            return;
        }

        SchemeViewModels.Add(new SchemeViewModel(scheme));
    }, _ => true);

    #endregion

    #region SaveExampleCommand

    private ICommand _saveExampleCommand;

    public ICommand SaveExampleCommand => _saveExampleCommand ??= new LambdaCommand(_ => { }, _ => true);

    #endregion

    #region TestCommand

    private ICommand _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ =>
    {
        _projectFileService.ReadFromFile(@"C:\Users\Vadim\Desktop\ExampleProject\ExampleProject.lsproj", out var proj);
    }, _ => true);

    #endregion
}