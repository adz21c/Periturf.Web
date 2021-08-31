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
using System.Threading.Tasks;
using Periturf.Web.BodyReaders;

namespace Periturf.Web.Verification
{
    /// <summary>
    /// Specifies how to process a web request.
    /// </summary>
    public interface IWebRequestEventSpecification
    {
        /// <summary>
        /// Builds the web request handler.
        /// </summary>
        /// <param name="defaultBodyReaderSpecification">The default specification for a body reader.</param>
        /// <returns></returns>
        Func<IWebRequestEvent, ValueTask<bool>> Build(IWebBodyReaderSpecification defaultBodyReaderSpecification);
    }
}
