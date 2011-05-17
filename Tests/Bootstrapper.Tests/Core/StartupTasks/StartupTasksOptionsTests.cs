using System;
using System.Collections.Generic;
using Bootstrap.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.StartupTasks
{
    [TestClass]
    public class StartupTasksOptionsTests
    {
        [TestMethod]
        public void ShouldCreateANewStartupTasksOptions()
        {
            //Act
            var result = new StartupTasksOptions();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTasksOptions));
        }

        [TestMethod]
        public void ShouldReturnAnEmptySequence()
        {
            //Act
            var options = new StartupTasksOptions();
            var result = options.Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Type>));
            options.UsingSequence(new SequenceSpecification().First<TaskAlpha>().Then<TaskBeta>());
        }

        [TestMethod]
        public void ShouldReturnASequenceSpecification()
        {
            //Act
            var options = new StartupTasksOptions();
            var result = options.Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Type>));
        }


    }
}
