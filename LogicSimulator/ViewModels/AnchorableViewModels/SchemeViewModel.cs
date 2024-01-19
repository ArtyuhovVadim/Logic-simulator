﻿using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;
using LogicSimulator.ViewModels.StatusViewModels;
using LogicSimulator.ViewModels.StatusViewModels.Base;
using SharpDX;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels;

public class SchemeViewModel : DocumentViewModel
{
    private readonly DockingViewModel _dockingViewModel;
    private readonly Scheme _scheme;

    private readonly SchemeStatusViewModel _statusViewModel;

    private readonly IEditorSelectionService _editorSelectionService;

    public SchemeViewModel(Scheme scheme, DockingViewModel dockingViewModel, IEditorSelectionService editorSelectionService)
    {
        _scheme = scheme;
        _dockingViewModel = dockingViewModel;
        _editorSelectionService = editorSelectionService;
        Objects = new ObservableCollection<BaseObjectViewModel>(_scheme.Objects);

        _statusViewModel = new SchemeStatusViewModel(this);

        _objects.CollectionChanged += (_, _) => _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.ObjectsCount));

        IconSource = new Uri("pack://application:,,,/Resources/Icons/scheme-icon16x16.png");
    }

    #region ToolsViewModel

    private SchemeToolsViewModel? _toolsViewModel;

    public SchemeToolsViewModel ToolsViewModel => _toolsViewModel ??= new SchemeToolsViewModel(this);

    #endregion

    #region Title

    public override string Title
    {
        get => _scheme.Name;
        set
        {
            _scheme.Name = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Objects

    private ObservableCollection<BaseObjectViewModel> _objects = [];

    public ObservableCollection<BaseObjectViewModel> Objects
    {
        get => _objects;
        private set => Set(ref _objects, value);
    }

    #endregion

    #region Scale

    private float _scale = 1f;

    public float Scale
    {
        get => _scale;
        set
        {
            if (Set(ref _scale, value))
            {
                _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.Scale));
            }
        }
    }

    #endregion

    #region Translation

    private Vector2 _translation = Vector2.Zero;

    public Vector2 Translation
    {
        get => _translation;
        set => Set(ref _translation, value);
    }

    #endregion

    #region MousePosition

    private Vector2 _mousePosition = Vector2.Zero;

    public Vector2 MousePosition
    {
        get => _mousePosition;
        set
        {
            if (Set(ref _mousePosition, value))
            {
                _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.MousePosition));
            }
        }
    }

    #endregion

    #region GridStep

    private float _gridStep = 25;

    public float GridStep
    {
        get => _gridStep;
        set => Set(ref _gridStep, value);
    }

    #endregion

    #region GridWidth

    private float _gridWidth = 2970;

    public float GridWidth
    {
        get => _gridWidth;
        set => Set(ref _gridWidth, value);
    }

    #endregion

    #region GridHeight

    private float _gridHeight = 2100;

    public float GridHeight
    {
        get => _gridHeight;
        set => Set(ref _gridHeight, value);
    }

    #endregion

    public override BaseStatusViewModel StatusViewModel => _statusViewModel;

    #region ObjectSelectedCommand

    private ICommand? _objectSelectedCommand;

    public ICommand ObjectSelectedCommand => _objectSelectedCommand ??= new LambdaCommand(OnSelectedObjectsChanged);

    #endregion

    public void SelectedObjectsChanged() => OnSelectedObjectsChanged();

    protected override void OnDocumentActivated() => OnSelectedObjectsChanged();

    protected override void OnDocumentDeactivated() => _editorSelectionService.SetEmptyEditor();

    protected override void OnClose(object? p) => _dockingViewModel.RemoveDocumentViewModel(this);

    private void OnSelectedObjectsChanged()
    {
        var selectedObjects = Objects.Where(x => x.IsSelected).ToArray();

        _statusViewModel.RaisedPropertyChanged(nameof(SchemeStatusViewModel.SelectedObjectsCount));

        if (selectedObjects.Length == 0)
        {
            _editorSelectionService.SetSchemeEditor(this);
            return;
        }

        _editorSelectionService.SetObjectsEditor(selectedObjects);
    }
}