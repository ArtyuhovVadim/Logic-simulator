using LogicSimulator.ViewModels.Status;
using LogicSimulator.ViewModels.Status.Base;
using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.Anchorable.Base;

public abstract class DocumentViewModel : AnchorableViewModel
{
    public virtual BaseStatusViewModel StatusViewModel { get; } = new EmptyStatusViewModel();

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

    protected virtual bool OnCanClose() => true;

    protected abstract void OnClose();

    protected virtual void OnDocumentActivated() { }

    protected virtual void OnDocumentDeactivated() { }
}