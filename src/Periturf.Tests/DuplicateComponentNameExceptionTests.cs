using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Periturf.Tests
{
    [TestFixture]
    class DuplicateComponentNameExceptionTests
    {
        [Test]
        public void Given_HostName_When_Ctor_Then_ExceptionCreated()
        {
            // Arrange
            const string componentName = "ComponentName";

            // Act
            var sut = new DuplicateComponentNameException(componentName);

            // Assert
            Assert.IsNull(sut.InnerException);
            Assert.AreEqual(componentName, sut.ComponentName);
            // Has the default message
            Assert.IsNotNull(sut.Message);
            Assert.IsNotEmpty(sut.Message);
        }

        [Test]
        public void Given_CustomMessageAndComponentName_When_Ctor_Then_ExceptionCreated()
        {
            // Arrange
            const string componentName = "ComponentName";
            const string message = "My Custom Mesage";

            // Act
            var sut = new DuplicateComponentNameException(message, componentName);

            // Assert
            Assert.IsNull(sut.InnerException);
            Assert.AreEqual(componentName, sut.ComponentName);
            Assert.AreEqual(message, sut.Message);
        }

        [Test]
        public void Given_AnException_When_SeriaizedAndDeserialized_Then_DataMatchesTheOriginal()
        {
            // Arrange
            const string componentName = "ComponentName";
            var originalException = new DuplicateComponentNameException(componentName);

            var buffer = new byte[4096];
            var ms = new MemoryStream(buffer);
            var ms2 = new MemoryStream(buffer);
            var formatter = new BinaryFormatter();

            // Act
            formatter.Serialize(ms, originalException);
            var deserializedException = (DuplicateComponentNameException)formatter.Deserialize(ms2);

            // Assert
            Assert.AreEqual(componentName, deserializedException.ComponentName);
            Assert.AreEqual(originalException.Message, deserializedException.Message);
        }
    }
}
