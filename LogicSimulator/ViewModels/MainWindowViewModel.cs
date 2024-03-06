using System.IO;
using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.StatusViewModels.Base;
using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;
    private readonly ISchemeFileService _schemeFileService;
    private readonly IProjectViewModelFactory _projectFactory;

    private readonly DockingViewModel _dockingViewModel;

    private readonly PropertiesViewModel _propertiesViewModel;
    private readonly ProjectExplorerViewModel _projectExplorerViewModel;
    private readonly MessagesOutputViewModel _messagesOutputViewModel;

    public MainWindowViewModel(
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        ISchemeFileService schemeFileService,
        IProjectViewModelFactory projectFactory,
        DockingViewModel dockingViewModel,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel,
        MessagesOutputViewModel messagesOutputViewModel)
    {
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;
        _schemeFileService = schemeFileService;
        _projectFactory = projectFactory;

        _dockingViewModel = dockingViewModel;
        _propertiesViewModel = propertiesViewModel;
        _projectExplorerViewModel = projectExplorerViewModel;
        _messagesOutputViewModel = messagesOutputViewModel;

        projectExplorerViewModel.SchemeOpened += OnSchemeOpened;

        _dockingViewModel.AddToolViewModel(_propertiesViewModel, true);
        _dockingViewModel.AddToolViewModel(_projectExplorerViewModel, true);
        _dockingViewModel.AddToolViewModel(_messagesOutputViewModel, true);

        _dockingViewModel.ActiveDocumentViewModelChanged += OnActiveDocumentViewModelChanged;
    }

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

    #region CurrentStatusViewModel

    public BaseStatusViewModel? CurrentStatusViewModel => _dockingViewModel.ActiveDocumentViewModel?.StatusViewModel;

    #endregion

    #region DockingViewModel

    public DockingViewModel DockingViewModel => _dockingViewModel;

    #endregion

    #region LoadExampleCommand

    private ICommand? _openFileCommand;

    public ICommand OpenFileCommand => _openFileCommand ??= new LambdaCommand(_ =>
    {
        try
        {
            if (_userDialogService.OpenFileDialog("Выберите файл", [("Проект", $"*{Project.Extension}")], out var projectPath) == UserDialogResult.Cancel)
                return;

            if (!_projectFileService.ReadFromFile(projectPath, out var project))
            {
                _userDialogService.ShowErrorMessage("Ошибка загрузки проекта", $"Не удалось загрузить файл по пути: {projectPath}");
                return;
            }

            var schemeFiles = project!.FileInfo!.Directory!.GetFiles($"*{Scheme.Extension}");
            var schemes = new List<Scheme>();

            foreach (var schemeFile in schemeFiles)
            {
                if (!_schemeFileService.ReadFromFile(schemeFile.FullName, out var scheme))
                {
                    _userDialogService.ShowErrorMessage("Ошибка загрузки схемы", $"Не удалось загрузить файл по пути: {schemeFile.FullName}");
                    continue;
                }

                schemes.Add(scheme!);
            }

            project.Schemes = schemes;

            var projectViewModel = _projectFactory.Create(project);

            _dockingViewModel.CloseAllDocumentsViewModel();

            ActiveProjectViewModel = projectViewModel;
        }
        catch (Exception e)
        {
            _userDialogService.ShowErrorMessage("Непредвиденная ошибка", e.Message);
        }
    });

    #endregion

    #region SaveFileCommand

    private ICommand? _saveFileCommand;

    public ICommand SaveFileCommand => _saveFileCommand ??= new LambdaCommand(() =>
    {
        try
        {
            if (_userDialogService.OpenFolderDialog("Выберите файл", out var projectDirPath) == UserDialogResult.Cancel)
                return;

            var project = ActiveProjectViewModel!.Model;
            var projectPath = Path.Combine(projectDirPath, project.FileInfo!.Name);

            if (!_projectFileService.SaveToFile(projectPath, project))
            {
                _userDialogService.ShowErrorMessage("Ошибка сохранения проекта", $"Не удалось сохранить файл по пути: {projectPath}");
                return;
            }

            foreach (var scheme in project.Schemes)
            {
                var schemePath = Path.Combine(projectDirPath, scheme.FileInfo!.Name);

                if (!_schemeFileService.SaveToFile(schemePath, scheme))
                {
                    _userDialogService.ShowErrorMessage("Ошибка сохранения схемы", $"Не удалось сохранить файл по пути: {scheme.FileInfo.FullName}");
                    return;
                }
            }
        }
        catch (Exception e)
        {
            _userDialogService.ShowErrorMessage("Непредвиденная ошибка", e.Message);
        }
    }, () => ActiveProjectViewModel is not null);

    #endregion

    private void OnSchemeOpened(SchemeViewModel scheme) => _dockingViewModel.AddOrSelectDocumentViewModel(scheme);

    private void OnActiveDocumentViewModelChanged(DocumentViewModel? oldDocument, DocumentViewModel? newDocument) => OnPropertyChanged(nameof(CurrentStatusViewModel));
}