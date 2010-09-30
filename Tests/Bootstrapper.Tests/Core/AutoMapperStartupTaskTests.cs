using System.Collections.Generic;
using AutoMapper;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bootstrap.Tests.Core
{
    [TestClass]
    public class AutoMapperStartupTaskTests
    {
        [TestMethod]
        public void ShouldCreateAnAutoMapperStartupTask()
        {
            //Act
            var result = new AutoMapperStartupTask();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AutoMapperStartupTask));
        }

        [TestMethod]
        public void ShouldInvokeTheCreateMapMethodOfAllIMapCreatorTypes()
        {
            //Arrange
            var mapCreator = new Mock<IMapCreator>();
            var locator = new Mock<IServiceLocator>();
            locator.Setup(l => l.GetAllInstances<IMapCreator>()).Returns(new List<IMapCreator> {mapCreator.Object});
            ServiceLocator.SetLocatorProvider(() => locator.Object);

            //Act
            new AutoMapperStartupTask().Run();

            //Assert
            mapCreator.Verify(m => m.CreateMap(), Times.Once());
        }

        [TestMethod]
        public void ShouldClearAllMaps()
        {
            //Arrange
            Mapper.CreateMap<AutoMapperStartupTaskTests, AutoMapperStartupTaskTests>();

            //Act
            new AutoMapperStartupTask().Reset();

            //Assert
            Assert.AreEqual(0, Mapper.GetAllTypeMaps().Length);
        }
    }
}
