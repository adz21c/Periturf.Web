/*
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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Web.RequestCriteria;

namespace Periturf.Web.Tests.RequestCriteria.Logical
{
    [TestFixture]
    class NotWebRequestCriteriaTests
    {
        private Func<IWebRequestEvent, bool> _criteria;
        private IWebRequestCriteriaSpecification<IWebRequestEvent> _criteriaSpec;

        [SetUp]
        public void SetUp()
        {
            _criteria = A.Fake<Func<IWebRequestEvent, bool>>();
            _criteriaSpec = A.Fake<IWebRequestCriteriaSpecification<IWebRequestEvent>>();
            A.CallTo(() => _criteriaSpec.Build()).Returns(_criteria);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Given_Response_When_Not_Then_ResultFlipped(bool nextResult, bool sutResult)
        {
            A.CallTo(() => _criteria.Invoke(A<IWebRequestEvent>._)).Returns(nextResult);
            
            var spec = new NotWebRequestCriteriaSpecification<IWebRequestEvent>();
            spec.AddCriteriaSpecification(_criteriaSpec);
            var sut = spec.Build();
            var result = sut(A.Dummy<IWebRequestEvent>());

            Assert.That(result, Is.EqualTo(sutResult));
        }
    }
}
