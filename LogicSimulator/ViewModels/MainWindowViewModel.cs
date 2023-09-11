using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;
    private readonly ProjectExplorerViewModel _projectExplorerViewModel;

    public MainWindowViewModel(
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        //PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel
        )
    {
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;
        _projectExplorerViewModel = projectExplorerViewModel;

        //propertiesViewModel.IsVisible = true;
        projectExplorerViewModel.IsVisible = true;

        projectExplorerViewModel.SchemeOpened += OnSchemeOpened;

        //AnchorableViewModels.Add(propertiesViewModel);
        AnchorableViewModels.Add(projectExplorerViewModel);
    }

    // #region TestSchemeViewModel
    //
    // private SchemeViewModel _testSchemeViewModel;
    //
    // public SchemeViewModel TestSchemeViewModel
    // {
    //     get => _testSchemeViewModel;
    //     set => Set(ref _testSchemeViewModel, value);
    // }
    //
    // #endregion

    #region ActiveContent

    private AnchorableViewModel _activeAnchorable;

    public AnchorableViewModel ActiveContent
    {
        get => _activeAnchorable;
        set => Set(ref _activeAnchorable, value);
    }

    #endregion

    #region AnchorableViewModels

    private ObservableCollection<AnchorableViewModel> _anchorableViewModels = new();

    public ObservableCollection<AnchorableViewModel> AnchorableViewModels
    {
        get => _anchorableViewModels;
        private set => Set(ref _anchorableViewModels, value);
    }

    #endregion

    #region ActiveProjectViewModel

    private ProjectViewModel _activeProjectViewModel;

    public ProjectViewModel ActiveProjectViewModel
    {
        get => _activeProjectViewModel;
        set
        {
            if (Set(ref _activeProjectViewModel, value))
            {
                _projectExplorerViewModel.ProjectViewModel = value;
            }
        }
    }

    #endregion

    #region OpenedSchemes

    private ObservableCollection<SchemeViewModel> _openedSchemes = new();

    public ObservableCollection<SchemeViewModel> OpenedSchemes
    {
        get => _openedSchemes;
        set => Set(ref _openedSchemes, value);
    }

    #endregion

    #region LoadExampleCommand

    private ICommand _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        var projectPath = @"C:\Users\Vadim\Desktop\ExampleProject\ExampleProject.lsproj";

        if (!_projectFileService.ReadFromFile(projectPath, out var project))
        {
            _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {projectPath}");
            return;
        }

        ActiveProjectViewModel = new ProjectViewModel(project);
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
        // new SchemeFileService().ReadFromFile("Data/Example.lss", out var scheme);
        // TestSchemeViewModel = new SchemeViewModel(scheme);
    }, _ => true);

    #endregion

    private void OnSchemeOpened(SchemeViewModel scheme)
    {
        if (!OpenedSchemes.Contains(scheme))
        {
            OpenedSchemes.Add(scheme);
        }

        scheme.IsActive = true;
    }
}