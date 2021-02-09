using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    public interface IWebRequestCriteriaSpecification
    {

        Func<IWebRequestEvent, bool> Build();
    }
}
