namespace LogicSimulator.Infrastructure.Factories.Interfaces;

public interface IMappedViewModelFactory<in TBaseModel, TBaseViewModel>
{
    void Register<TConcreteModel>(Func<TConcreteModel, TBaseViewModel> factory) where TConcreteModel : notnull, TBaseModel;

    TBaseViewModel Create(TBaseModel model);
}