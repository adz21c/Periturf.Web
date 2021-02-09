using Microsoft.AspNetCore.Http;
using Periturf.Events;
using Periturf.Web.Configuration.Requests.Responses;
using Periturf.Web.RequestCriteria;
using System;

namespace Periturf.Web.Configuration.Requests
{
    public interface IWebRequestEventConfigurator : IEventConfigurator<IWebRequest>
    {
        void AddCriteriaSpecification(IWebRequestCriteriaSpecification spec);

        void SetResponseSpecification(IWebRequestResponseSpecification spec);
    }
}
