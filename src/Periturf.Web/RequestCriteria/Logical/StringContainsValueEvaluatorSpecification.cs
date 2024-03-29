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

namespace Periturf.Web.RequestCriteria.Logical
{
    class StringContainsValueEvaluatorSpecification : IValueEvaluatorSpecification<string>, IValueConditionBuilder<string>
    {
        private IValueEvaluatorSpecification<string>? _next;
        private readonly string _comparisonValue;

        public StringContainsValueEvaluatorSpecification(string comparisonValue)
        {
            _comparisonValue = comparisonValue;
        }

        public void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<string> spec)
        {
            _next = spec;
        }

        public Func<string, bool> Build()
        {
            var next = _next?.Build();
            return value => (value?.Contains(_comparisonValue, StringComparison.OrdinalIgnoreCase) ?? false) && (next?.Invoke(value) ?? true);
        }
    }
}
