using LogicSimulator.ViewModels.AnchorableViewModels.Base;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class DockingViewModel : BindableBase
{
    public event Action<DocumentViewModel?, DocumentViewModel?>? ActiveDocumentViewModelChanged;

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

    public void CloseAllDocumentsViewModel()
    {
        _documentViewModels.Clear();
        OnPropertyChanged(nameof(DocumentViewModels));
        ActiveDocumentViewModel = null;
    }

    public void CloseDocumentViewModel(DocumentViewModel documentViewModel)
    {
        _documentViewModels.Remove(documentViewModel);

        OnPropertyChanged(nameof(DocumentViewModels));

        if (ActiveDocumentViewModel == documentViewModel)
        {
            ActiveDocumentViewModel = _documentViewModels.Any() ? _documentViewModels.Last() : null;
        }
    }

    public void AddDocumentViewModel(DocumentViewModel documentViewModel, bool isSelected = false)
    {
        if (_documentViewModels.Contains(documentViewModel))
            throw new ApplicationException($"{documentViewModel.GetType()} already added.");

        documentViewModel.IsSelected = isSelected;

        _documentViewModels.Add(documentViewModel);

        OnPropertyChanged(nameof(DocumentViewModels));

        if (isSelected)
        {
            ActiveDocumentViewModel = documentViewModel;
        }
    }

    public void AddOrSelectDocumentViewModel(DocumentViewModel documentViewModel)
    {
        if (!_documentViewModels.Contains(documentViewModel))
            _documentViewModels.Add(documentViewModel);

        documentViewModel.IsSelected = true;

        OnPropertyChanged(nameof(DocumentViewModels));

        ActiveDocumentViewModel = documentViewModel;
    }

    public void AddToolViewModel(ToolViewModel toolViewModel, bool isVisible = false)
    {
        if (_toolViewModels.Contains(toolViewModel))
            throw new ApplicationException($"{toolViewModel.GetType()} already added.");

        toolViewModel.IsVisible = isVisible;

        _toolViewModels.Add(toolViewModel);

        OnPropertyChanged(nameof(ToolViewModels));
    }

    public void OpenAndSelectToolViewModel(ToolViewModel toolViewModel)
    {
        toolViewModel.IsVisible = true;
        toolViewModel.IsSelected = true;
    }

    public void OpenToolViewModel(ToolViewModel toolViewModel)
    {
        toolViewModel.IsVisible = true;
    }

    public void CloseToolViewModel(ToolViewModel toolViewModel)
    {
        toolViewModel.IsVisible = false;
        toolViewModel.IsSelected = false;
    }

    public void SelectAnchorableViewModel(AnchorableViewModel anchorableViewModel)
    {
        anchorableViewModel.IsSelected = true;
    }

    public void UnselectAnchorableViewModel(AnchorableViewModel anchorableViewModel)
    {
        anchorableViewModel.IsSelected = false;
    }
}