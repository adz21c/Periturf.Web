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
using System.Threading;
using System.Threading.Tasks;
using Periturf.Verify;

namespace Periturf.Web.Verification
{
    class WebConditionSpecification : IConditionSpecification
    {
        private readonly IWebVerificationManager _webVerificationManager;
        private readonly IWebRequestEventSpecification _webRequestEventSpecification;

        public WebConditionSpecification(string componentName, string conditionDescription, IWebVerificationManager webVerificationManager, IWebRequestEventSpecification webRequestEventSpecification)
        {
            ComponentName = componentName;
            ConditionDescription = conditionDescription;
            _webVerificationManager = webVerificationManager;
            _webRequestEventSpecification = webRequestEventSpecification;
        }

        public string ComponentName { get; }

        public string ConditionDescription { get; }

        public Task<IConditionFeed> BuildAsync(IConditionInstanceFactory conditionInstanceFactory, CancellationToken ct)
        {
            var matcher = _webRequestEventSpecification.Build();

            var verifier = new WebVerifier(matcher, _webVerificationManager, conditionInstanceFactory);

            _webVerificationManager.Register(verifier);

            return Task.FromResult<IConditionFeed>(verifier);
        }
    }
}
