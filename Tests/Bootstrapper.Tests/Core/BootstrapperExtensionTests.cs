using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrapper.Tests.Core
{
    [TestClass]
    public class BootstrapperExtensionTests
    {
        [TestMethod]
        public void ShouldCreateABoostrapperExtension()
        {
            //Act
            var result = new BootstrapperExtension();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BootstrapperExtension));

        }
    }
}
