//
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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Periturf.Verify;
using Periturf.Web.BodyReaders;

namespace Periturf.Web.Verification
{
    public class ConditionBuilder : IConditionBuilder
    {
        private readonly string _componentName;
        private readonly IWebVerificationManager _verificationManager;
        private readonly IWebBodyReaderSpecification _defaultBodyReaderSpecification;

        internal ConditionBuilder(string componentName, IWebVerificationManager verificationManager, IWebBodyReaderSpecification defaultBodyReaderSpecification)
        {
            _componentName = componentName;
            _verificationManager = verificationManager;
            _defaultBodyReaderSpecification = defaultBodyReaderSpecification;
        }

        public IConditionSpecification CreateWebRequestEventConditionSpecification(IWebRequestEventSpecification spec)
        {
            return new WebConditionSpecification(_componentName, "", _verificationManager, spec, _defaultBodyReaderSpecification);
        }
    }
}
