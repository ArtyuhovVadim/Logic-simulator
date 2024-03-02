﻿using LogicSimulator.Infrastructure.Services.Interfaces;
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
    private readonly IProjectViewModelFactory _projectFactory;

    private readonly DockingViewModel _dockingViewModel;

    private readonly PropertiesViewModel _propertiesViewModel;
    private readonly ProjectExplorerViewModel _projectExplorerViewModel;
    private readonly MessagesOutputViewModel _messagesOutputViewModel;

    public MainWindowViewModel(
        IUserDialogService userDialogService,
        IProjectFileService projectFileService,
        IProjectViewModelFactory projectFactory,
        DockingViewModel dockingViewModel,
        PropertiesViewModel propertiesViewModel,
        ProjectExplorerViewModel projectExplorerViewModel,
        MessagesOutputViewModel messagesOutputViewModel)
    {
        _userDialogService = userDialogService;
        _projectFileService = projectFileService;
        _projectFactory = projectFactory;

        _dockingViewModel = dockingViewModel;
        _propertiesViewModel = propertiesViewModel;
        _projectExplorerViewModel = projectExplorerViewModel;
        _messagesOutputViewModel = messagesOutputViewModel;

        projectExplorerViewModel.SchemeOpened += OnSchemeOpened;

        _dockingViewModel
            .AddToolViewModel(_propertiesViewModel, true)
            .AddToolViewModel(_projectExplorerViewModel, true)
            .AddToolViewModel(_messagesOutputViewModel, true);

        _dockingViewModel.ActiveDocumentViewModelChanged += OnActiveDocumentViewModelChanged;

        // TODO: Временно для быстрой загрузки тестового проекта
        LoadExampleCommand.Execute(null);
        _dockingViewModel.AddOrSelectDocumentViewModel(ActiveProjectViewModel!.Schemes.First());
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

    private ICommand? _loadExampleCommand;

    public ICommand LoadExampleCommand => _loadExampleCommand ??= new LambdaCommand(_ =>
    {
        const string projectPath = @"Data\ExampleProject\ExampleProject.lsproj";

        if (!_projectFileService.ReadFromFile(projectPath, out var project))
        {
            _userDialogService.ShowErrorMessage("Ошибка загрузки файла", $"Не удалось загрузить файл: {projectPath}");
            return;
        }

        ActiveProjectViewModel = _projectFactory.Create(project!);
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

    private void OnSchemeOpened(SchemeViewModel scheme) => _dockingViewModel.AddOrSelectDocumentViewModel(scheme);

    private void OnActiveDocumentViewModelChanged(DocumentViewModel? oldDocument, DocumentViewModel? newDocument) => OnPropertyChanged(nameof(CurrentStatusViewModel));
}