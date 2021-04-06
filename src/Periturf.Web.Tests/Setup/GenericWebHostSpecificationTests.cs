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
using System.Collections.Generic;
using FakeItEasy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Periturf.Web.Setup;

namespace Periturf.Web.Tests.Setup
{
    class GenericWebHostSpecificationTests
    {
        [Test]
        public void Given_Null_When_AddWebComponentSpec_Then_Exception()
        {
            var sut = new GenericWebHostSpecification();
            Assert.That(() => sut.AddWebComponentSpecification(null), Throws.ArgumentNullException.With.Property("ParamName").EqualTo("spec"));
        }

        [Test]
        public void Given_NoSpecs_When_Apply_Then_NoComponents()
        {
            var builder = A.Dummy<IHostBuilder>();

            var sut = new GenericWebHostSpecification();
            var components = sut.Apply(builder);

            Assert.That(components, Is.Empty);
        }

        [Test]
        public void Given_Specs_When_Apply_Then_Components()
        {
            var builder = A.Dummy<IHostBuilder>();

            var component1 = A.Dummy<Periturf.Components.IComponent>();
            var component1Spec = A.Fake<IWebComponentSetupSpecification>();
            var component1ConfigureApp = A.Dummy<Action<IApplicationBuilder>>();
            var component1ConfigureServices = A.Dummy<Action<IServiceCollection>>();
            A.CallTo(() => component1Spec.Name).Returns(nameof(component1));
            A.CallTo(() => component1Spec.Path).Returns("/" + nameof(component1));
            A.CallTo(() => component1Spec.Configure()).Returns(new ConfigureWebAppDto(component1, component1ConfigureApp, component1ConfigureServices));

            var component2 = A.Dummy<Periturf.Components.IComponent>();
            var component2Spec = A.Fake<IWebComponentSetupSpecification>();
            var component2ConfigureApp = A.Dummy<Action<IApplicationBuilder>>();
            var component2ConfigureServices = A.Dummy<Action<IServiceCollection>>();
            A.CallTo(() => component2Spec.Name).Returns(nameof(component2));
            A.CallTo(() => component2Spec.Path).Returns("/" + nameof(component2));
            A.CallTo(() => component2Spec.Configure()).Returns(new ConfigureWebAppDto(component2, component2ConfigureApp, component2ConfigureServices));

            var sut = new GenericWebHostSpecification();
            sut.AddWebComponentSpecification(component1Spec);
            sut.AddWebComponentSpecification(component2Spec);

            var components = sut.Apply(builder);

            Assert.That(components.GetValueOrDefault(nameof(component1)), Is.Not.Null);
            Assert.That(components.GetValueOrDefault(nameof(component2)), Is.Not.Null);
        }
    }
}
