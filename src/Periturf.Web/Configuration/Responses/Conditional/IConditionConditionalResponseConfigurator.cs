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

using System;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Configuration.Responses.Conditional
{
    /// <summary>
    /// Configures a response condition.
    /// </summary>
    /// <typeparam name="TWebRequestEvent">The type of the web request event.</typeparam>
    public interface IConditionConditionalResponseConfigurator<TWebRequestEvent> : IWebResponseConfigurable<TWebRequestEvent> where TWebRequestEvent : IWebRequestEvent
    {
        /// <summary>
        /// The criteria a request must meet to trigger this condition's response.
        /// </summary>
        /// <param name="config">The configuration.</param>
        void Criteria(Action<IWebRequestCriteriaConfigurator<TWebRequestEvent>> config);
    }
}
