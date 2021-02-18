using Periturf.Events;
using Periturf.Web.Configuration.Requests.Responses;
using Periturf.Web.Interpretation;
using Periturf.Web.RequestCriteria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Periturf.Web.Configuration.Requests
{
    class WebRequestEventSpecification : EventSpecification<IWebRequestEvent>, IWebRequestEventConfigurator<IWebRequestEvent>, IWebRequestEventSpecification
    {
        private IWebRequestCriteriaSpecification<IWebRequestEvent>? _criteriaSpecification;
        private IWebRequestResponseSpecification? _responseSpecification;

        public WebRequestEventSpecification(IEventHandlerFactory eventHandlerFactory) : base(eventHandlerFactory)
        { }

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<IWebRequestEvent> spec)
        {
            _criteriaSpecification = spec;
        }

        public void SetResponseSpecification(IWebRequestResponseSpecification spec)
        {
            _responseSpecification = spec;
        }

        public IWebConfiguration Build()
        {
            Debug.Assert(_criteriaSpecification != null, "_criteriaSpecification != null");
            Debug.Assert(_responseSpecification != null, "_responseSpecification != null");

            return new WebConfiguration(
                _criteriaSpecification.Build(),
                _responseSpecification.BuildFactory(),
                CreateHandler());
        }
    }

    class WebRequestEventSpecification<TBody> : EventSpecification<IWebRequestEvent<TBody>>, IWebRequestEventConfigurator<IWebRequestEvent<TBody>>, IWebRequestEventSpecification
    {
        private IWebRequestCriteriaSpecification<IWebRequestEvent<TBody>>? _criteriaSpecification;
        private IWebRequestResponseSpecification? _responseSpecification;

        public WebRequestEventSpecification(IEventHandlerFactory eventHandlerFactory) : base(eventHandlerFactory)
        { }

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<IWebRequestEvent<TBody>> spec)
        {
            _criteriaSpecification = spec;
        }

        public void SetResponseSpecification(IWebRequestResponseSpecification spec)
        {
            _responseSpecification = spec;
        }

        public IWebConfiguration Build()
        {
            Debug.Assert(_criteriaSpecification != null, "_criteriaSpecification != null");
            Debug.Assert(_responseSpecification != null, "_responseSpecification != null");

            return new WebConfiguration<TBody>(
                _criteriaSpecification.Build(),
                _responseSpecification.BuildFactory(),
                CreateHandler(),
                new DeserializationBodyInterpretor<TBody>(new Serialization.JsonSerializer()));
        }
    }
}
