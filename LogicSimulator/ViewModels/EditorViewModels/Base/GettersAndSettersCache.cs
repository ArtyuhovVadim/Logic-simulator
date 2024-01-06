using System.Linq.Expressions;

namespace LogicSimulator.ViewModels.EditorViewModels.Base;

public static class GettersAndSettersCache
{
    private static readonly Dictionary<(string, Type), Func<object, object>> GettersMap = [];

    private static readonly Dictionary<(string, Type), Action<object, object>> SettersMap = [];

    public static Func<object, object> GetGetter(string propertyName, object obj)
    {
        var objType = obj.GetType();
        var key = (propertyName, objType);

        if (!GettersMap.TryGetValue(key, out var getter))
        {
            var propertyParameter = Expression.Parameter(typeof(object), "Object");
            getter = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Property(Expression.Convert(propertyParameter, objType), propertyName), typeof(object)), propertyParameter).Compile();
            GettersMap[key] = getter;
        }

        return getter;
    }

    public static Action<object, object> GetSetter<TProperty>(string propertyName, object obj)
    {
        var objType = obj.GetType();
        var key = (propertyName, objType);

        if (!SettersMap.TryGetValue(key, out var setter))
        {
            var propertyParameter = Expression.Parameter(typeof(object), "Object");
            var valueParameter = Expression.Parameter(typeof(object), "Value");
            setter = Expression.Lambda<Action<object, object>>(
                Expression.Assign(Expression.Property(Expression.Convert(propertyParameter, objType), propertyName), Expression.Convert(valueParameter, typeof(TProperty))),
                propertyParameter, valueParameter).Compile();
            SettersMap[key] = setter;
        }

        return setter;
    }

    public static Action<object, object> GetSetter(string propertyName, object obj, Type valueType)
    {
        var objType = obj.GetType();
        var key = (propertyName, objType);

        if (!SettersMap.TryGetValue(key, out var setter))
        {
            var propertyParameter = Expression.Parameter(typeof(object), "Object");
            var valueParameter = Expression.Parameter(typeof(object), "Value");
            setter = Expression.Lambda<Action<object, object>>(
                Expression.Assign(Expression.Property(Expression.Convert(propertyParameter, objType), propertyName), Expression.Convert(valueParameter, valueType)),
                propertyParameter, valueParameter).Compile();
            SettersMap[key] = setter;
        }

        return setter;
    }
}