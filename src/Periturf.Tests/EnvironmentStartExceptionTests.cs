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
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Periturf.Tests
{
    [TestFixture]
    class EnvironmentStartExceptionTests
    {
        [Test]
        public void Given_NoHostErrors_When_Ctor_Then_ExceptionCreated()
        {
            // Act
            var sut = new EnvironmentStartException();

            // Assert
            Assert.IsNull(sut.InnerException);
            // Not host details
            Assert.IsNotNull(sut.Details);
            Assert.IsEmpty(sut.Details);
            // Has the default message
            Assert.IsNotNull(sut.Message);
            Assert.IsNotEmpty(sut.Message);
        }

        [Test]
        public void Given_HostErrors_When_Ctor_Then_ExceptionCreated()
        {
            // Arrange
            var hostDetails = new[] { new HostExceptionDetails("MyHost", new Exception()) };

            // Act
            var sut = new EnvironmentStartException(hostDetails);

            // Assert
            Assert.IsNull(sut.InnerException);
            // host details
            Assert.AreEqual(hostDetails, sut.Details);
            // Has the default message
            Assert.IsNotNull(sut.Message);
            Assert.IsNotEmpty(sut.Message);
        }

        [Test]
        public void Given_CustomMessageAndNoHostErrors_When_Ctor_Then_ExceptionCreated()
        {
            // Arrange
            const string message = "My Custom Error Message";

            // Act
            var sut = new EnvironmentStartException(message);

            // Assert
            Assert.IsNull(sut.InnerException);
            // Not host details
            Assert.IsNotNull(sut.Details);
            Assert.IsEmpty(sut.Details);
            Assert.AreEqual(message, sut.Message);
        }

        [Test]
        public void Given_CustomMessageAndHostErrors_When_Ctor_Then_ExceptionCreated()
        {
            // Arrange
            const string message = "My Custom Error Message";
            var hostDetails = new[] { new HostExceptionDetails("MyHost", new Exception()) };

            // Act
            var sut = new EnvironmentStartException(message, hostDetails);

            // Assert
            Assert.IsNull(sut.InnerException);
            // Not host details
            Assert.AreEqual(hostDetails, sut.Details);
            Assert.AreEqual(message, sut.Message);
        }

        [Test]
        public void Given_AnException_When_SeriaizedAndDeserialized_Then_DataMatchesTheOriginal()
        {
            // Arrange
            var hostDetails = new[] { new HostExceptionDetails("MyHost", new Exception("MyMessage")) };
            var originalException = new EnvironmentStartException(hostDetails);

            var buffer = new byte[4096];
            var ms = new MemoryStream(buffer);
            var ms2 = new MemoryStream(buffer);
            var formatter = new BinaryFormatter();

            // Act
            formatter.Serialize(ms, originalException);
            var deserializedException = (EnvironmentStartException)formatter.Deserialize(ms2);

            // Assert
            Assert.IsNotNull(deserializedException.Details);
            Assert.IsNotEmpty(deserializedException.Details);

            var orignalHostExceptionDetails = originalException.Details.Single();
            var deserializedHostExceptionDetails = deserializedException.Details.Single();
            Assert.AreEqual(orignalHostExceptionDetails.HostName, deserializedHostExceptionDetails.HostName);
            Assert.IsNotNull(deserializedHostExceptionDetails.Exception);
            Assert.AreEqual(orignalHostExceptionDetails.Exception.Message, deserializedHostExceptionDetails.Exception.Message);

            Assert.AreEqual(originalException.Message, deserializedException.Message);
        }
    }
}
