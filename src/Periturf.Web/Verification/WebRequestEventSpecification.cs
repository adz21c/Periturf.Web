﻿//
//   Copyright 2021 Adam Burton (adz21c@gmail.com)
//   
//   Licensed under the Apache License, Version 2.0 (the "License")
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//  
//

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Periturf.Web.BodyReaders;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Verification
{
    class WebRequestEventSpecification : IWebRequestEventConfigurator, IWebRequestEventSpecification
    {
        private IWebRequestCriteriaSpecification<IWebRequestEvent>? _criteriaSpecification;

        public void AddCriteriaSpecification(IWebRequestCriteriaSpecification<IWebRequestEvent> spec)
        {
            _criteriaSpecification = spec;
        }

        public Func<IWebRequestEvent, ValueTask<bool>> Build(IWebBodyReaderSpecification defaultBodyReaderSpecification)
        {
            Debug.Assert(_criteriaSpecification != null);
            var matcher = _criteriaSpecification.Build();

            return e => new ValueTask<bool>(matcher(e));
        }
    }
}
