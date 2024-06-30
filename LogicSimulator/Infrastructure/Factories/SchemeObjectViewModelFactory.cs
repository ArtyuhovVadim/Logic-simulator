using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Models.Base;
using LogicSimulator.ViewModels.ObjectViewModels.Base;

namespace LogicSimulator.Infrastructure.Factories;

public class SchemeObjectViewModelFactory : IMappedViewModelFactory<BaseObjectModel, BaseObjectViewModel>
{
    private readonly Dictionary<Type, Func<BaseObjectModel, BaseObjectViewModel>> _factoriesMap = [];

    public void Register<TConcreteModel>(Func<TConcreteModel, BaseObjectViewModel> factory) where TConcreteModel : BaseObjectModel
    {
        if (!_factoriesMap.TryAdd(typeof(TConcreteModel), model => factory((TConcreteModel)model)))
            throw new InvalidOperationException($"{typeof(TConcreteModel).Name} already registered.");
    }

    public BaseObjectViewModel Create(BaseObjectModel model)
    {
        if (!_factoriesMap.TryGetValue(model.GetType(), out var factory))
            throw new InvalidOperationException($"{model.GetType().Name} is not registered in factory.");

        return factory(model);
    }
}