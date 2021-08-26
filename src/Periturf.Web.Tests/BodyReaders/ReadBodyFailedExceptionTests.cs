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

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Periturf.Web.BodyReaders;

namespace Periturf.Web.Tests.BodyReaders
{
    class ReadBodyFailedExceptionTests
    {
        [Test]
        public void Given_NoHostErrors_When_Ctor_Then_ExceptionCreated()
        {
            // Act
            var sut = new ReadBodyFailedException();

            // Assert
            Assert.That(sut.InnerException, Is.Null);
            // Has the default message
            Assert.That(sut.Message, Is.Not.Null);
            Assert.That(sut.Message, Is.Not.Empty);
        }

        [Test]
        public void Given_CustomMessage_When_Ctor_Then_ExceptionCreated()
        {
            // Arrange
            const string message = "My Custom Error Message";

            // Act
            var sut = new ReadBodyFailedException(message);

            // Assert
            Assert.That(sut.InnerException, Is.Null);
            Assert.That(sut.Message, Is.EqualTo(message));
        }

        [Test]
        public void Given_AnException_When_SeriaizedAndDeserialized_Then_DataMatchesTheOriginal()
        {
            // Arrange
            var originalException = new ReadBodyFailedException();

            var buffer = new byte[4096];
            using var ms = new MemoryStream(buffer);
            using var ms2 = new MemoryStream(buffer);
            var formatter = new BinaryFormatter();

            // Act
            formatter.Serialize(ms, originalException);
            var deserializedException = (ReadBodyFailedException)formatter.Deserialize(ms2);

            // Assert
            Assert.That(deserializedException, Is.Not.Null);
        }
    }
}
