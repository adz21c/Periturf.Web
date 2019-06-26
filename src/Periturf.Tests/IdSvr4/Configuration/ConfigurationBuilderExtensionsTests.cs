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
using Periturf.IdSvr4;
using System;

namespace Periturf.Tests.IdSvr4.Configuration
{
    [TestFixture]
    class ConfigurationBuilderExtensionsTests
    {
        [Test]
        public void Given_ConfigurationBuilder_When_ConfigureIdSvr4_Then_IdSvr4ComponentAdded()
        {
            // Arrange
            var config = A.Dummy<Action<IdSvr4Configurator>>();
            var builder = A.Fake<IConfiugrationBuilder>();
            const string componentName = "IdSvr4";
            A.CallTo(() => builder.AddComponentConfigurator(componentName, A<Func<IdSvr4Component, IComponentConfigurator>>._))
                .Invokes(x => x.GetArgument<Func<IdSvr4Component, IComponentConfigurator>>(1).Invoke(null));


            // Act
            builder.ConfigureIdSvr4(componentName, config);

            // Assert
            A.CallTo(() => builder.AddComponentConfigurator(componentName, A<Func<IdSvr4Component, IComponentConfigurator>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => config.Invoke(A<IdSvr4Configurator>._)).MustHaveHappenedOnceExactly();
        }
    }
}
