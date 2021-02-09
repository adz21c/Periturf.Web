using Periturf.Events;
using Periturf.Web.Configuration.Requests.Responses;
using Periturf.Web.RequestCriteria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Periturf.Web.Configuration.Requests
{
    class WebRequestEventSpecification : EventSpecification<IWebRequest>, IWebRequestEventConfigurator
    {
        private IWebRequestCriteriaSpecification? _criteriaSpecification;
        private IWebRequestResponseSpecification? _responseSpecification;

        public WebRequestEventSpecification(IEventHandlerFactory eventHandlerFactory) : base(eventHandlerFactory)
        { }

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification spec)
        {
            _criteriaSpecification = spec;
        }

        public void SetResponseSpecification(IWebRequestResponseSpecification spec)
        {
            _responseSpecification = spec;
        }

        public WebConfiguration Build()
        {
            Debug.Assert(_criteriaSpecification != null, "_criteriaSpecification != null");
            Debug.Assert(_responseSpecification != null, "_responseSpecification != null");

            return new WebConfiguration(
                _criteriaSpecification.Build(),
                _responseSpecification.BuildFactory(),
                CreateHandler());
        }
    }
}
