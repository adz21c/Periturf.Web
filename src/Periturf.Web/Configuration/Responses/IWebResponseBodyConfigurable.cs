using System;
using System.Collections.Generic;
using System.Text;

namespace Periturf.Web.Configuration.Responses
{
    public interface IWebResponseBodyConfigurable<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        void AddWebResponseBodySpecification(IWebResponseBodySpecification<TWebRequestEvent> spec);
    }
}
