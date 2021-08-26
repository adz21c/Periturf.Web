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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.BodyReaders;
using Periturf.Web.RequestCriteria;
using Periturf.Web.Verification;

namespace Periturf.Web.Tests.Verification
{
    class WebRequestEventSpecificationTests
    {
        private WebRequestEventSpecification _spec;
        private Func<IWebRequestEvent, bool> _criteria;
        private Func<IWebRequestEvent, ValueTask<bool>> _matcher;

        [SetUp]
        public void SetUp()
        {
            _criteria = A.Fake<Func<IWebRequestEvent, bool>>();
            
            var criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => criteriaSpec.Build()).Returns(_criteria);

            _spec = new WebRequestEventSpecification();
            _spec.AddCriteriaSpecification(criteriaSpec);
            _matcher = _spec.Build(A.Dummy<IWebBodyReaderSpecification>());
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task Given_ValidMatcher_When_Invoke_Then_Result(bool expectedResult)
        {
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).Returns(expectedResult);

            var result = await _matcher(A.Dummy<IWebRequestEvent>());

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
