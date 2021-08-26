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
using Periturf.Web.BodyWriters;

namespace Periturf.Web.Configuration.Requests
{
    /// <summary>
    /// Builds a <see cref="IWebConfiguration"/> from the specification.
    /// </summary>
    public interface IWebRequestEventSpecification
    {
        /// <summary>
        /// Factory method for a <see cref="IWebConfiguration"/>.
        /// </summary>
        /// <param name="defaultBodyReaderSpec">The default body reader specification for the web component.</param>
        /// <param name="defaultBodyWriterSpec">The default body writer specification for the web component.</param>
        /// <returns></returns>
        IWebConfiguration Build(IWebBodyReaderSpecification defaultBodyReaderSpec, IWebBodyWriterSpecification defaultBodyWriterSpec);
    }
}