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
using System.Collections.Generic;
using System.Linq;

namespace Periturf.Web.BodyWriters.Conditional
{
    class ConditionalBodyWriterSpecification : IWebBodyWriterSpecification, IConditionalBodyWriterConfigurator
    {
        private readonly List<ConditionConditionalBodyWriterSpecification> _conditionSpecifications = new List<ConditionConditionalBodyWriterSpecification>();

        public void Condition(Action<IConditionConditionalBodyWriterConfigurator> config)
        {
            var spec = new ConditionConditionalBodyWriterSpecification();
            config(spec);
            _conditionSpecifications.Add(spec);
        }

        public IBodyWriter Build()
        {
            return new ConditionalBodyWriter(_conditionSpecifications.Select(x => x.Build()).ToList());
        }
    }
}
