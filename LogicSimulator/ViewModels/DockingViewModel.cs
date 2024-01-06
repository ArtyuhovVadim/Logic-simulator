using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using System.Collections.Specialized;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class DockingViewModel : BindableBase
{
    public event Action<DocumentViewModel?, DocumentViewModel?>? ActiveDocumentViewModelChanged;

    public DockingViewModel() => _documentViewModels.CollectionChanged += OnDocumentCollectionChanged;

    #region ActiveDocumentViewModel

    private DocumentViewModel? _activeDocumentViewModel;

    public DocumentViewModel? ActiveDocumentViewModel
    {
        get => _activeDocumentViewModel;
        set
        {
            var tmp = _activeDocumentViewModel;

            if (Set(ref _activeDocumentViewModel, value))
            {
                if (tmp is not null)
                {
                    tmp.IsActiveDocument = false;
                }

                if (value is not null)
                {
                    value.IsActiveDocument = true;
                }

                ActiveDocumentViewModelChanged?.Invoke(tmp, value);
            }
        }
    }

    #endregion

    #region DocumentViewModels

    private ObservableCollection<DocumentViewModel> _documentViewModels = [];

    public IEnumerable<DocumentViewModel> DocumentViewModels
    {
        get => _documentViewModels;
        private set => Set(ref _documentViewModels, new ObservableCollection<DocumentViewModel>(value));
    }

    #endregion

    #region ToolViewModels

    private ObservableCollection<ToolViewModel> _toolViewModels = [];

    public IEnumerable<ToolViewModel> ToolViewModels
    {
        get => _toolViewModels;
        private set => Set(ref _toolViewModels, new ObservableCollection<ToolViewModel>(value));
    }

    #endregion

    public DockingViewModel AddDocumentViewModel(DocumentViewModel documentViewModel, bool isSelected = false)
    {
        if (_documentViewModels.Contains(documentViewModel))
            throw new ApplicationException($"{documentViewModel.GetType()} already added.");

        documentViewModel.IsSelected = isSelected;

        _documentViewModels.Add(documentViewModel);

        OnPropertyChanged(nameof(DocumentViewModels));

        return this;
    }

    public DockingViewModel RemoveDocumentViewModel(DocumentViewModel documentViewModel)
    {
        _documentViewModels.Remove(documentViewModel);

        OnPropertyChanged(nameof(DocumentViewModels));

        return this;
    }

    public DockingViewModel AddOrSelectDocumentViewModel(DocumentViewModel documentViewModel)
    {
        if (!_documentViewModels.Contains(documentViewModel))
            _documentViewModels.Add(documentViewModel);

        documentViewModel.IsSelected = true;

        OnPropertyChanged(nameof(DocumentViewModels));

        return this;
    }

    public DockingViewModel AddToolViewModel(ToolViewModel toolViewModel, bool isVisible = false)
    {
        if (_toolViewModels.Contains(toolViewModel))
            throw new ApplicationException($"{toolViewModel.GetType()} already added.");

        toolViewModel.IsVisible = isVisible;

        _toolViewModels.Add(toolViewModel);

        OnPropertyChanged(nameof(ToolViewModels));

        return this;
    }

    public DockingViewModel OpenAndSelectToolViewModel(ToolViewModel toolViewModel)
    {
        toolViewModel.IsVisible = true;
        toolViewModel.IsSelected = true;

        return this;
    }

    public DockingViewModel OpenToolViewModel(ToolViewModel toolViewModel)
    {
        toolViewModel.IsVisible = true;

        return this;
    }

    public DockingViewModel CloseToolViewModel(ToolViewModel toolViewModel)
    {
        toolViewModel.IsVisible = false;
        toolViewModel.IsSelected = false;

        return this;
    }

    public DockingViewModel SelectAnchorableViewModel(AnchorableViewModel anchorableViewModel)
    {
        anchorableViewModel.IsSelected = true;

        return this;
    }

    public DockingViewModel UnselectAnchorableViewModel(AnchorableViewModel anchorableViewModel)
    {
        anchorableViewModel.IsSelected = false;

        return this;
    }

    private void OnDocumentCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Remove) return;

        if (!_documentViewModels.Any())
        {
            ActiveDocumentViewModel = null;
            return;
        }

        ActiveDocumentViewModel = _documentViewModels.Last();
    }
}