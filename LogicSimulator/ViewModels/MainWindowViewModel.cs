using System.Collections.ObjectModel;
using System.Windows.Input;
using LogicSimulator.Infrastructure.Commands;
using LogicSimulator.Infrastructure.Services;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Scene;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.Base;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;

    public MainWindowViewModel(
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel)
    {
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;

        propertiesViewModel.IsVisible = true;

        AnchorableViewModels.Add(propertiesViewModel);
        AnchorableViewModels.Add(projectExplorerViewModel);
    }

    #region ActiveContent

    private BindableBase _activeContent;

    public BindableBase ActiveContent
    {
        get => _activeContent;
        set => Set(ref _activeContent, value);
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

    #region ActiveProjectViewModel

    private ProjectViewModel _activeProjectViewModel;

    public ProjectViewModel ActiveProjectViewModel
    {
        get => _activeProjectViewModel;
        set => Set(ref _activeProjectViewModel, value);
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

    public ICommand TestCommand => _testCommand ??= new LambdaCommand(_ => { }, _ => true);
    #endregion
}