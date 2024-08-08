using System.IO;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Common;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Anchorable.Base;
using LogicSimulator.ViewModels.Status.Base;
using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase, IRecipient<DocumentClosingMessage>
{
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;
    private readonly ISchemeFileService _schemeFileService;
    private readonly IProjectViewModelFactory _projectFactory;
    private readonly IMessageBus _messageBus;

    private readonly DockingViewModel _dockingViewModel;

    private readonly PropertiesViewModel _propertiesViewModel;
    private readonly ProjectExplorerViewModel _projectExplorerViewModel;
    private readonly MessagesOutputViewModel _messagesOutputViewModel;
    private readonly TimelineViewModel _timelineViewModel;

    public MainWindowViewModel(
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        ISchemeFileService schemeFileService,
        IProjectViewModelFactory projectFactory,
        IMessageBus messageBus,
        DockingViewModel dockingViewModel,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel,
        MessagesOutputViewModel messagesOutputViewModel,
        TimelineViewModel timelineViewModel)
    {
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;
        _schemeFileService = schemeFileService;
        _projectFactory = projectFactory;
        _messageBus = messageBus;

        _dockingViewModel = dockingViewModel;
        _propertiesViewModel = propertiesViewModel;
        _projectExplorerViewModel = projectExplorerViewModel;
        _messagesOutputViewModel = messagesOutputViewModel;
        _timelineViewModel = timelineViewModel;

        projectExplorerViewModel.SchemeOpened += OnSchemeOpened;

        _dockingViewModel.AddToolViewModel(_propertiesViewModel, true);
        _dockingViewModel.AddToolViewModel(_projectExplorerViewModel, true);
        _dockingViewModel.AddToolViewModel(_timelineViewModel, true);
        _dockingViewModel.AddToolViewModel(_messagesOutputViewModel, true);

        _dockingViewModel.ActiveDocumentViewModelChanged += OnActiveDocumentViewModelChanged;

        _messageBus.RegisterHandler(this, RefType.Strong);
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

    public ICommand OpenFileCommand => _openFileCommand ??= new LambdaCommand(() =>
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

    public void Receive(DocumentClosingMessage message) => _dockingViewModel.CloseDocumentViewModel(message.Document);

    private void OnSchemeOpened(SchemeViewModel scheme) => _dockingViewModel.AddOrSelectDocumentViewModel(scheme);

    private void OnActiveDocumentViewModelChanged(DocumentViewModel? oldDocument, DocumentViewModel? newDocument) => OnPropertyChanged(nameof(CurrentStatusViewModel));
}