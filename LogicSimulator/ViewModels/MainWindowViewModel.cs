using System.IO;
using LogicSimulator.Infrastructure.ExtensionMethods;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Infrastructure.Messages;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Common;
using LogicSimulator.ViewModels.Anchorable;
using LogicSimulator.ViewModels.Anchorable.Base;
using LogicSimulator.ViewModels.Status.Base;
using Microsoft.Extensions.Logging;
using WpfExtensions.Mvvm;
using WpfExtensions.Mvvm.Commands;
using WpfExtensions.Mvvm.Messaging;

namespace LogicSimulator.ViewModels;

public class MainWindowViewModel : BindableBase, IRecipient<DocumentClosingMessage>, IRecipient<DocumentOpenedMessage>
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IUserDialogService _userDialogService;
    private readonly IProjectFileService _projectFileService;
    private readonly ISchemeFileService _schemeFileService;
    private readonly IProjectViewModelFactory _projectFactory;
    private readonly IMessageBus _messageBus;

    private readonly DockingViewModel _dockingViewModel;

    public MainWindowViewModel(
        ILogger<MainWindowViewModel> logger,
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
        _logger = logger;
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;
        _schemeFileService = schemeFileService;
        _projectFactory = projectFactory;
        _messageBus = messageBus;

        _dockingViewModel = dockingViewModel;

        _dockingViewModel.AddToolViewModel(propertiesViewModel, true);
        _dockingViewModel.AddToolViewModel(projectExplorerViewModel, true);
        _dockingViewModel.AddToolViewModel(timelineViewModel, true);
        _dockingViewModel.AddToolViewModel(messagesOutputViewModel, true);

        _dockingViewModel.ActiveDocumentViewModelChanged += OnActiveDocumentViewModelChanged;

        _messageBus.RegisterHandler<DocumentOpenedMessage>(this, RefType.Strong);
        _messageBus.RegisterHandler<DocumentClosingMessage>(this, RefType.Strong);
    }

    #region ActiveProjectViewModel

    private ProjectViewModel? _activeProjectViewModel;

    public ProjectViewModel? ActiveProjectViewModel
    {
        get => _activeProjectViewModel;
        set => Set(ref _activeProjectViewModel, value);
    }

    #endregion

    #region ActiveSchemeViewModel

    public SchemeViewModel? ActiveSchemeViewModel => DockingViewModel.ActiveDocumentViewModel as SchemeViewModel;

    #endregion

    #region CurrentStatusViewModel

    public BaseStatusViewModel? CurrentStatusViewModel => _dockingViewModel.ActiveDocumentViewModel?.StatusViewModel;

    #endregion

    #region DockingViewModel

    public DockingViewModel DockingViewModel => _dockingViewModel;

    #endregion

    #region OpenFileCommand

    private ICommand? _openFileCommand;

    public ICommand OpenFileCommand => _openFileCommand ??= new AsyncLambdaCommand(async () =>
    {
        try
        {
            if (_userDialogService.OpenFileDialog("Выберите файл", [("Проект", $"*{Project.Extension}")], out var projectPath) == UserDialogResult.Cancel)
                return;

            _logger.LogInformation("Project loading has started.");

            Project project;

            try
            {
                project = await _projectFileService.ReadFromFileAsync(projectPath);
                _logger.LogInformation("Project file ({projectName}) has been loaded successfully.", project.Name);
            }
            catch (Exception e)
            {
                _logger.LogError("Can not load project file: {projectPath}\nInternal error:\n{e}", projectPath, e);
                _userDialogService.ShowErrorMessage("Ошибка загрузки проекта", $"Не удалось загрузить файл по пути: {projectPath}\nВнутренняя ошибка:\n{e}");
                return;
            }

            var projectFileInfo = new FileInfo(projectPath);

            var schemeFiles = projectFileInfo.Directory!.GetFiles($"*{Scheme.Extension}");
            var schemes = new List<Scheme>();

            foreach (var schemeFile in schemeFiles)
            {
                try
                {
                    schemes.Add(await _schemeFileService.ReadFromFileAsync(schemeFile.FullName));
                    _logger.LogInformation("Scheme file ({schemeFile}) has been loaded successfully.", schemeFile.Name);
                }
                catch (Exception e)
                {
                    _logger.LogError("Can not load scheme file: {schemePath}\nInternal error:\n{e}", schemeFile.FullName, e);
                    _userDialogService.ShowErrorMessage("Ошибка загрузки схемы", $"Не удалось загрузить файл по пути: {schemeFile.FullName}\nВнутренняя ошибка:\n{e}");
                }
            }

            project.Schemes = schemes;

            _logger.LogInformation("Project has been loaded successfully.");

            var projectViewModel = _projectFactory.Create(project);

            _dockingViewModel.CloseAllDocumentsViewModel();

            ActiveProjectViewModel = projectViewModel;
            _messageBus.Send(new ProjectLoadedMessage(projectViewModel));
        }
        catch (Exception e)
        {
            _logger.LogInformation("Unexpected error while loading project:\n{e}", e);
            _userDialogService.ShowErrorMessage("Непредвиденная ошибка", e.Message);
        }
    });

    #endregion

    #region SaveFileCommand

    private ICommand? _saveFileCommand;

    public ICommand SaveFileCommand => _saveFileCommand ??= new AsyncLambdaCommand(async () =>
    {
        try
        {
            if (_userDialogService.OpenFolderDialog("Выберите файл", out var projectDirPath) == UserDialogResult.Cancel)
                return;

            _logger.LogInformation("Project saving has started.");

            var project = ActiveProjectViewModel!.Model;
            var projectPath = Path.Combine(projectDirPath, project.Name);

            try
            {
                await _projectFileService.SaveToFileAsync(projectPath, project);
                _logger.LogInformation("Project file ({projectName}) has been saved successfully.", project.Name);
            }
            catch (Exception e)
            {
                _logger.LogError("Can not save project file: {projectPath}\nInternal error:\n{e}", projectPath, e);
                _userDialogService.ShowErrorMessage("Ошибка сохранения проекта", $"Не удалось сохранить файл по пути: {projectPath}\nВнутренняя ошибка:\n{e}");
                return;
            }

            foreach (var scheme in project.Schemes)
            {
                var schemeFilePath = Path.Combine(projectDirPath, scheme.Name);

                try
                {
                    await _schemeFileService.SaveToFileAsync(schemeFilePath, scheme);
                    _logger.LogInformation("Scheme file ({schemeFile}) has been saved successfully.", scheme.Name);
                }
                catch (Exception e)
                {
                    _logger.LogError("Can not save scheme file: {schemePath}\nInternal error:\n{e}", schemeFilePath, e);
                    _userDialogService.ShowErrorMessage("Ошибка сохранения схемы", $"Не удалось сохранить файл по пути: {schemeFilePath}\nВнутренняя ошибка:\n{e}");
                }
            }

            _logger.LogInformation("Project has been saved successfully.");
        }
        catch (Exception e)
        {
            _logger.LogInformation("Unexpected error while saving project:\n{e}", e);
            _userDialogService.ShowErrorMessage("Непредвиденная ошибка", e.Message);
        }
    }, () => ActiveProjectViewModel is not null);

    #endregion

    public void Receive(DocumentOpenedMessage message) => _dockingViewModel.AddOrSelectDocumentViewModel(message.Document);

    public void Receive(DocumentClosingMessage message) => _dockingViewModel.CloseDocumentViewModel(message.Document);

    private void OnActiveDocumentViewModelChanged(DocumentViewModel? oldDocument, DocumentViewModel? newDocument)
    {
        OnPropertyChanged(nameof(ActiveSchemeViewModel));
        OnPropertyChanged(nameof(CurrentStatusViewModel));
    }
}