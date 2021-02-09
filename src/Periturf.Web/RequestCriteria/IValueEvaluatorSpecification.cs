using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    public interface IValueEvaluatorSpecification<T>
    {
        Func<T, bool> Build();
    }
}
