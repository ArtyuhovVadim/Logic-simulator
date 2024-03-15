using LogicSimulator.Infrastructure;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;
using WpfExtensions.Mvvm;

namespace LogicSimulator.ViewModels;

public class ProjectViewModel : BindableBase, IModelBased<Project>
{
    public ProjectViewModel(Project project, ISchemeViewModelFactory schemeFactory)
    {
        Model = project;
        _schemes = new ObservableCollectionEx<SchemeViewModel, Scheme>(Model.Schemes, schemeFactory.Create);
    }

    #region Model

    public Project Model { get; }

    #endregion
    
    #region Name

    public string Name => Model.FileInfo?.Name ?? throw new InvalidOperationException();

    #endregion

    #region Schemes

    private readonly ObservableCollectionEx<SchemeViewModel, Scheme> _schemes;

    public ObservableCollection<SchemeViewModel> Schemes => _schemes;

    #endregion
}