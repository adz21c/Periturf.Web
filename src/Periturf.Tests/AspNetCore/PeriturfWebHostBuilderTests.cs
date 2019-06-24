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
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using Periturf.Components;
using System.Collections.Generic;

namespace Periturf.Tests.AspNetCore
{
    [TestFixture]
    class PeriturfWebHostBuilderTests
    {
        [Test]
        public void Given_MultipleComponents_When_WebHost_Then_NewHostRegisteredWithComponents()
        {
            // Arrange
            var configurator = A.Dummy<ISetupConfigurator>();
            var component1 = A.Dummy<IComponent>();
            var component2 = A.Dummy<IComponent>();
            const string hostName = "HostName";

            // Act
            configurator.WebHost(hostName, c =>
            {
                c.UseStartup<StartupDummy>();
                c.AddComponent(nameof(component1), component1);
                c.AddComponent(nameof(component2), component2);
            });

            // Assert
            A.CallTo(() => configurator.Host(
                hostName,
                A<IHost>.That.NullCheckedMatches(
                    x => x.Components.Count == 2 &&
                    x.Components.GetValueOrDefault(nameof(component1)) == component1 &&
                    x.Components.GetValueOrDefault(nameof(component2)) == component2,
                    x => x.Write("ComponentDictionary")))).MustHaveHappened();
        }

        private class StartupDummy
        {
#pragma warning disable S1186 // Methods should not be empty
#pragma warning disable S1144 // Unused private types or members should be removed
            public void Configure()
#pragma warning restore S1186 // Methods should not be empty
            { }
#pragma warning restore S1144 // Unused private types or members should be removed
        }
    }
}
