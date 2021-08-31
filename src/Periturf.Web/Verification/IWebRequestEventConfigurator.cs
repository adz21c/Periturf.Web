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

using Periturf.Web.BodyReaders;
using Periturf.Web.RequestCriteria;
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error

namespace Periturf.Web.Verification
{


    /// <summary>
    /// Define a web request criteria and how to respond.
    /// </summary>
    /// <seealso cref="Periturf.Web.RequestCriteria.IWebRequestCriteriaConfigurator{Periturf.Web.IWebRequestEvent}" />
    public interface IWebRequestEventConfigurator : IWebRequestCriteriaConfigurator<IWebRequestEvent>
    {
    }

    /// <summary>
    /// Define a web request criteria and how to respond.
    /// </summary>
    /// <typeparam name="TBody">The type of the body.</typeparam>
    /// <seealso cref="Periturf.Web.RequestCriteria.IWebRequestCriteriaConfigurator{Periturf.Web.IWebRequestEvent}" />
    public interface IWebRequestEventConfigurator<TBody> : IWebRequestCriteriaConfigurator<IWebRequestEvent<TBody>>, IWebBodyReaderConfigurator
    {
    }
}
