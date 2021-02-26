using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Web.RequestCriteria
{
    public interface IWebRequestCriteriaSpecification<TWebRequestEvent>
        where TWebRequestEvent : IWebRequestEvent
    {
        Func<TWebRequestEvent, bool> Build();
    }
}
