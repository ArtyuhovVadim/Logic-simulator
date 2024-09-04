using LogicSimulator.Infrastructure.Collections;
using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.Models.Common;
using LogicSimulator.ViewModels.Anchorable;
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

    public string Name
    {
        get => Model.Name;
        set => Set(Model.Name, value, Model, (model, value) => model.Name = value);
    }

    #endregion

    #region Schemes

    private readonly ObservableCollectionEx<SchemeViewModel, Scheme> _schemes;

    public ObservableCollection<SchemeViewModel> Schemes => _schemes;

    #endregion
}