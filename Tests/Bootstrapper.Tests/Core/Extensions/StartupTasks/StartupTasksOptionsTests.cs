using System.Collections.Generic;
using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
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
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
        }

        [TestMethod]
        public void ShouldReturnASequenceSpecification()
        {
            //Act
            var options = new StartupTasksOptions();
            options.UsingThisExecutionOrder(s => s.First<TaskAlpha>().Then<TaskBeta>());
            var result = options.Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
            Assert.AreEqual(typeof(TaskAlpha), result[0].TaskType);
            Assert.AreEqual(typeof(TaskBeta), result[1].TaskType);
        }

        [TestMethod]
        public void ShouldReturnAListOfSequenceWithOneSequence()
        {
            //Act
            var options = new StartupTasksOptions();
            var result = options.Groups;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<ISequenceSpecification>));
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
            Assert.IsInstanceOfType(result[0], typeof(SequenceSpecification));
        }

        [TestMethod]
        public void ShouldReturnTheFirstSequence()
        {
            //Act
            var options = new StartupTasksOptions();
            var result = options.Sequence;

            //Assert
            Assert.AreSame(options.Groups[0].Sequence, result);
        }

        [TestMethod]
        public void ShouldNotAddANewSequenceIfTheFirstSequenceIsEmptyUsingWithGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();

            //Act
            options.WithGroup(s => null);
            var result = options.Groups;

            //Assert
            Assert.AreEqual(1, result.Count);
        }


        [TestMethod]
        public void ShouldAddANewSequenceUsingWithGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();

            //Act
            options.UsingThisExecutionOrder(s => s.First<TaskAlpha>())
                    .WithGroup(s => s.First<TaskBeta>());
            var result = options.Groups;

            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ShouldReturnTheLastSequenceUsingWithGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();
            options.UsingThisExecutionOrder(s => s.First<TaskAlpha>())
                .WithGroup(s => s.First<TaskBeta>());

            //Act
            var result = options.Sequence;

            //Assert
            Assert.AreEqual(options.Groups[1].Sequence, result);
        }

        [TestMethod]
        public void ShouldReturnAStartupTaskOptionsUsingWithGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();
            options.UsingThisExecutionOrder(s => s.First<TaskAlpha>());

            //Act
            var result = options.WithGroup(s => s.First<TaskBeta>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTasksOptions));
            Assert.AreSame(options, result);            
        }

        [TestMethod]
        public void ShouldNotAddANewSequenceIfTheFirstSequenceIsEmptyUsingAndGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();

            //Act
            options.AndGroup(s => null);
            var result = options.Groups;

            //Assert
            Assert.AreEqual(1, result.Count);
        }


        [TestMethod]
        public void ShouldAddANewSequenceUsingAndGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();

            //Act
            options.WithGroup(s => s.First<TaskAlpha>())
                .AndGroup(s => s.First<TaskBeta>());
            var result = options.Groups;

            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void ShouldReturnTheLastSequenceUsingAndGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();
            options.WithGroup(s => s.First<TaskAlpha>())
                .AndGroup(s => s.First<TaskBeta>());

            //Act
            var result = options.Sequence;

            //Assert
            Assert.AreEqual(options.Groups[1].Sequence, result);
        }

        [TestMethod]
        public void ShouldReturnAStartupTaskOptionsUsingAndGroup()
        {
            //Arrange
            var options = new StartupTasksOptions();
            options.WithGroup(s => s.First<TaskAlpha>());

            //Act
            var result = options.AndGroup(s => s.First<TaskBeta>());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StartupTasksOptions));
            Assert.AreSame(options, result);
        }

        [TestMethod]
        public void ShouldSpecifySequenceForGroups()
        {
            //Act
            var options = new StartupTasksOptions();
            options.WithGroup(s => s.First<TaskAlpha>().Then<TaskBeta>())
                   .AndGroup(s => s.First<TaskGamma>().Then().TheRest());
            var result = options.Groups;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result[0], typeof(ISequenceSpecification));
            Assert.IsInstanceOfType(result[0], typeof(SequenceSpecification));
            Assert.AreEqual(typeof(TaskAlpha), result[0].Sequence[0].TaskType);
            Assert.AreEqual(typeof(TaskBeta), result[0].Sequence[1].TaskType);
            Assert.IsInstanceOfType(result[1], typeof(ISequenceSpecification));
            Assert.IsInstanceOfType(result[1], typeof(SequenceSpecification));
            Assert.AreEqual(typeof(TaskGamma), result[1].Sequence[0].TaskType);
            Assert.AreEqual(typeof(IStartupTask), result[1].Sequence[1].TaskType);
        }
    }
}
