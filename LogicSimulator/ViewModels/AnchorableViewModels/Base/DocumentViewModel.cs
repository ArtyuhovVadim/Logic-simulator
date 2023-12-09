using WpfExtensions.Mvvm.Commands;

namespace LogicSimulator.ViewModels.AnchorableViewModels.Base;

public abstract class DocumentViewModel : AnchorableViewModel
{
    #region CloseCommand

    private ICommand _closeCommand;

    public ICommand CloseCommand => _closeCommand ??= new LambdaCommand(Close, CanClose);

    #endregion

    protected virtual bool CanClose(object p) => true;

    protected abstract void Close(object p);
}