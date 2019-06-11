/*
 *     Copyright 2019 Adam Burton (adz21c@gmail.com)
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
using FakeItEasy;
using NUnit.Framework;
using Periturf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Periturf.Tests
{
    [TestFixture]
    class EnvironmentConfigurationTests
    {
        [Test]
        public void Given_Environment_When_ComponentConfiguredAdded_Then_ComponentIsConfigured()
        {
            var component = A.Fake<IComponent>();
            
            var componentConfigurator = A.Fake<IComponentConfigurator>();
            A.CallTo(() => componentConfigurator.Component).Returns(component);

            var host = A.Fake<IHost>();
            A.CallTo(() => host.Components).Returns(new List<IComponent> { component });

            var environment = Environment.Setup(x =>
            {
                x.Host(host);
            });

            environment.Configure(x => x.AddComponentConfigurator(componentConfigurator));

            A.CallTo(() => componentConfigurator.RegisterConfiguration(A<Guid>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => component.UnregisterConfiguration(A<Guid>._)).MustNotHaveHappened();
        }

        [Test]
        public void Given_EnvironmentWithConfiguration_When_ConfigurationDisposed_Then_ConfigurationRemoved()
        {
            var component = A.Fake<IComponent>();

            var componentConfigurator = A.Fake<IComponentConfigurator>();
            A.CallTo(() => componentConfigurator.Component).Returns(component);

            var host = A.Fake<IHost>();
            A.CallTo(() => host.Components).Returns(new List<IComponent> { component });

            var environment = Environment.Setup(x =>
            {
                x.Host(host);
            });

            var config = environment.Configure(x => x.AddComponentConfigurator(componentConfigurator));
            config.Dispose();

            A.CallTo(() => componentConfigurator.RegisterConfiguration(A<Guid>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => component.UnregisterConfiguration(A<Guid>._)).MustHaveHappenedOnceExactly();
        }
    }
}
