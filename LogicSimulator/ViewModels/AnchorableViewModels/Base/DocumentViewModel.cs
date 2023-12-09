using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels.Base;

public abstract class DocumentViewModel : AnchorableViewModel
{
    #region IsActiveDocument

    private bool _isActiveDocument;

    public bool IsActiveDocument
    {
        get => _isActiveDocument;
        set
        {
            if (Set(ref _isActiveDocument, value))
            {
                if (value) OnDocumentActivated();
                else OnDocumentDeactivated();
            }
        }
    }

    #endregion

    #region CloseCommand

    private ICommand? _closeCommand;

    public ICommand CloseCommand => _closeCommand ??= new LambdaCommand(OnClose, OnCanClose);

    #endregion

    protected virtual bool OnCanClose(object? p) => true;

    protected abstract void OnClose(object? p);

    protected virtual void OnDocumentActivated() { }

    protected virtual void OnDocumentDeactivated() { }
}