using System.Windows;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;
    private readonly ProjectExplorerViewModel _projectExplorerViewModel;

    public MainWindowViewModel(
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel)
    {
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;
        _projectExplorerViewModel = projectExplorerViewModel;

        propertiesViewModel.IsVisible = true;
        projectExplorerViewModel.IsVisible = true;

        projectExplorerViewModel.SchemeOpened += OnSchemeOpened;

        AnchorableViewModels.Add(propertiesViewModel);
        AnchorableViewModels.Add(projectExplorerViewModel);

        Task.Run(async () =>
        {
            await Task.Delay(100);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                LoadExampleCommand.Execute(null);
                OnSchemeOpened(ActiveProjectViewModel!.Schemes.First());
            });
        });
    }

    #region ActiveContent

    private AnchorableViewModel? _activeAnchorable;

    public AnchorableViewModel? ActiveContent
    {
        get => _activeAnchorable;
        set => Set(ref _activeAnchorable, value);
    }

    #endregion

    #region AnchorableViewModels

    private ObservableCollection<AnchorableViewModel> _anchorableViewModels = [];

    public ObservableCollection<AnchorableViewModel> AnchorableViewModels
    {
        get => _anchorableViewModels;
        private set => Set(ref _anchorableViewModels, value);
    }

    #endregion

    #region ActiveProjectViewModel

    private ProjectViewModel? _activeProjectViewModel;

    public ProjectViewModel? ActiveProjectViewModel
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

    private ObservableCollection<SchemeViewModel> _openedSchemes = [];

    public ObservableCollection<SchemeViewModel> OpenedSchemes
    {
        get => _openedSchemes;
        set => Set(ref _openedSchemes, value);
    }

    #endregion

    #region LoadExampleCommand

    private ICommand? _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        const string projectPath = @"Data\ExampleProject\ExampleProject.lsproj";

        if (!_projectFileService.ReadFromFile(projectPath, out var project))
        {
            _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {projectPath}");
            return;
        }

        ActiveProjectViewModel = new ProjectViewModel(project!);
    });

    #endregion

    #region SaveExampleCommand

    private ICommand? _saveExampleCommand;

    public ICommand SaveExampleCommand => _saveExampleCommand ??= new LambdaCommand(_ => { });

    #endregion

    #region TestCommand

    private ICommand? _testCommand;

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ => { });

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