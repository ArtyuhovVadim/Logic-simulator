using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LogicSimulator.Infrastructure.Converters;

public class CollectionToElementsCountConverter : Converter
{
    public override object Convert(object v, Type t, object p, CultureInfo c)
    {
        if (v is IEnumerable<object> collection)
        {
            return collection.Count();
        }

        return -1;
    }
}