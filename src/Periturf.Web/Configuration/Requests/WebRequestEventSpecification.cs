/*
 *     Copyright 2021 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Diagnostics;
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyWriters;
using Periturf.Web.Configuration.Responses;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Configuration.Requests
{
    class WebRequestEventSpecification : IWebRequestEventConfigurator<IWebRequestEvent>, IWebRequestEventSpecification
    {
        private IWebRequestCriteriaSpecification<IWebRequestEvent>? _criteriaSpecification;
        private IWebResponseSpecification<IWebRequestEvent>? _responseSpecification;

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<IWebRequestEvent> spec)
        {
            _criteriaSpecification = spec;
        }

        public void AddWebResponseSpecification(IWebResponseSpecification<IWebRequestEvent> spec)
        {
            _responseSpecification = spec;
        }

        public void AddWebBodyReaderSpecification(IWebBodyReaderSpecification spec)
        {
            // Method intentionally left empty. No body.
        }

        public IWebConfiguration Build(IWebBodyReaderSpecification defaultBodyReaderSpec, IWebBodyWriterSpecification defaultBodyWriterSpec)
        {
            Debug.Assert(_criteriaSpecification != null, "_criteriaSpecification != null");
            Debug.Assert(_responseSpecification != null, "_responseSpecification != null");

            return new WebConfiguration(
                _criteriaSpecification.Build(),
                _responseSpecification.BuildResponseWriter(defaultBodyWriterSpec));
        }
    }
}
