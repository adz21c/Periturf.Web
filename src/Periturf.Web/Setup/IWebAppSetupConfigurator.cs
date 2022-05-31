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
using Periturf.Web.BodyReaders;
using Periturf.Web.BodyWriters;

namespace Periturf.Web.Setup
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWebAppSetupConfigurator
    {
        /// <summary>
        /// Configures a body reader used by all request handlers unless otherwise specified.
        /// </summary>
        /// <param name="config"></param>
        void DefaultBodyReader(Action<IWebBodyReaderConfigurator> config);

        /// <summary>
        /// Configures a body writer used by all request handlers unless otherwise specified.
        /// </summary>
        /// <param name="config"></param>
        void DefaultBodyWriter(Action<IWebBodyWritableConfigurator> config);
    }
}