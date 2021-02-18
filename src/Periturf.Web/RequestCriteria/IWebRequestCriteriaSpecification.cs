using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.RequestCriteria
{
    public interface IWebRequestCriteriaSpecification<TWebRequestEvent>
        where TWebRequestEvent : IWebRequestEvent
    {
        Func<TWebRequestEvent, bool> Build();
    }
}
