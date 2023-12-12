using LogicSimulator.ViewModels.AnchorableViewModels;
using LogicSimulator.ViewModels.StatusViewModels.Base;
using SharpDX;

namespace LogicSimulator.ViewModels.StatusViewModels;

public class SchemeStatusViewModel : BaseStatusViewModel
{
    private readonly SchemeViewModel _scheme;

    public SchemeStatusViewModel(SchemeViewModel scheme) : base(scheme) => _scheme = scheme;

    #region MousePosition

    public Vector2 MousePosition => _scheme.MousePosition;

    #endregion

    #region Scale

    public float Scale => _scheme.Scale;

    #endregion

    #region ObjectsCount

    public int ObjectsCount => _scheme.Objects.Count();

    #endregion

    #region SelectedObjectsCount

    public int SelectedObjectsCount => _scheme.Objects.Count(x => x.IsSelected);

    #endregion
}