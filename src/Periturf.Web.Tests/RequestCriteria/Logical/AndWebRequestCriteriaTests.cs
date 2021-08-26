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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;
using Periturf.Web.RequestCriteria.Logical;

namespace Periturf.Web.Tests.RequestCriteria.Logical
{
    [TestFixture]
    class AndWebRequestCriteriaTests
    {
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void Given_Criterias_When_And_Then_Result(bool criteriaOneResult, bool criteriaTwoResult, bool sutResult)
        {
            var criteriaOne = A.Fake<Func<IWebRequestEvent, bool>>();
            A.CallTo(() => criteriaOne.Invoke(A<IWebRequestEvent>._)).Returns(criteriaOneResult);
            var criteriaOneSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => criteriaOneSpec.Build()).Returns(criteriaOne);

            var criteriaTwo = A.Fake<Func<IWebRequestEvent, bool>>();
            A.CallTo(() => criteriaTwo.Invoke(A<IWebRequestEvent>._)).Returns(criteriaTwoResult);
            var criteriaTwoSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => criteriaTwoSpec.Build()).Returns(criteriaTwo);

            var spec = new AndWebRequestCriteriaSpecification<IWebRequestEvent>();
            spec.AddCriteriaSpecification(criteriaOneSpec);
            spec.AddCriteriaSpecification(criteriaTwoSpec);

            var sut = spec.Build();

            var result = sut(A.Dummy<IWebRequestEvent>());

            Assert.That(result, Is.EqualTo(sutResult));

        }
    }
}
