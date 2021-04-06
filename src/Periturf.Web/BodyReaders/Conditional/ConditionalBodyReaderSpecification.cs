﻿/*
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Periturf.Web.BodyReaders.Conditional
{
    class ConditionalBodyReaderSpecification : IWebBodyReaderSpecification, IConditionalBodyReaderConfigurator
    {
        private readonly List<ConditionConditionalBodyReaderSpecification> _conditionSpecifications = new List<ConditionConditionalBodyReaderSpecification>();

        public void Condition(Action<IConditionConditionalBodyReaderConfigurator> config)
        {
            var spec = new ConditionConditionalBodyReaderSpecification();
            config(spec);
            _conditionSpecifications.Add(spec);
        }

        public IBodyReader Build()
        {
            return new ConditionalBodyReader(_conditionSpecifications.Select(x => x.Build()).ToList());
        }
    }
}
