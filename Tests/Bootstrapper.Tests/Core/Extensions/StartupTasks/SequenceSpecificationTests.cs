using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [TestClass]
    public class SequenceSpecificationTests
    {
        [TestMethod]
        public void ShouldCreateASequenceSpecification()
        {
            //Act
            var result = new SequenceSpecification();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequenceSpecification));
            Assert.IsInstanceOfType(result, typeof(SequenceSpecification));
        }

        [TestMethod]
        public void ShouldReturnAnEmptySequence()
        {
            //Arrange
            var spec = new SequenceSpecification();

            //Act
            var result = spec.Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldAddATaskToTheTopOfTheSequence()
        {
            //Arrange
            var spec = new SequenceSpecification();
            spec.Sequence.Add(new TaskExecutionParameters { TaskType = typeof(TaskBeta) });
            spec.Sequence.Add(new TaskExecutionParameters { TaskType = typeof(TaskOmega) });

            //Act
            spec.First<TaskAlpha>();
            var result = spec.Sequence;

            //Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(typeof(TaskAlpha), result[0].TaskType);
        }

        [TestMethod]
        public void ShouldReturnAFirstInSequenceSequenceSpecial()
        {
            //Arrange
            var spec = new SequenceSpecification();

            //Act            
            var result = spec.First();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequenceSpecial));
            Assert.IsInstanceOfType(result, typeof(SequenceSpecial));
            Assert.AreSame(spec,  result.SequenceSpecification);
            Assert.AreEqual(true, result.FirstInSequence);
        }

        [TestMethod]
        public void ShouldAddATaskToTheBottomOfTheSequence()
        {
            //Arrange
            var spec = new SequenceSpecification();
            spec.Sequence.Add(new TaskExecutionParameters { TaskType = typeof(TaskAlpha) });
            spec.Sequence.Add(new TaskExecutionParameters { TaskType = typeof(TaskBeta) });

            //Act
            spec.Then<TaskOmega>();
            var result = spec.Sequence;

            //Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(typeof(TaskOmega), result[result.Count-1].TaskType);
        }

        [TestMethod]
        public void ShouldReturnANonFirstInSequenceSequenceSpecial()
        {
            //Arrange
            var spec = new SequenceSpecification();

            //Act            
            var result = spec.Then();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ISequenceSpecial));
            Assert.IsInstanceOfType(result, typeof(SequenceSpecial));
            Assert.AreSame(spec, result.SequenceSpecification);
            Assert.AreEqual(false, result.FirstInSequence);
        }

        [TestMethod]
        public void ShouldSetTheDelayForTheFirstTask()
        {
            //Arrange
            var spec = new SequenceSpecification();

            //Act            
            var result = spec.First<TaskAlpha>().DelayStartBy(5)
                .First<TaskBeta>().DelayStartBy(10).Seconds
                .First<TaskOmega>().DelayStartBy(15).MilliSeconds
                .First().TheRest().DelayStartBy(20)
                .First<TestStartupTask>()
                .Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
            Assert.AreEqual(0, result.First(t => t.TaskType == typeof(TestStartupTask)).Delay);
            Assert.AreEqual(20, result.First(t => t.TaskType == typeof(IStartupTask)).Delay);
            Assert.AreEqual(15, result.First(t => t.TaskType == typeof(TaskOmega)).Delay);
            Assert.AreEqual(10000, result.First(t => t.TaskType == typeof(TaskBeta)).Delay);
            Assert.AreEqual(5, result.First(t => t.TaskType == typeof(TaskAlpha)).Delay);
        }

        [TestMethod]
        public void ShouldSetTheDelayForTheMiddleTask()
        {
            //Arrange
            var spec = new SequenceSpecification();

            //Act            
            var result = spec.Then<TaskAlpha>().DelayStartBy(5)
                .Then<TaskBeta>().DelayStartBy(10).Seconds
                .Then<TaskOmega>().DelayStartBy(15).MilliSeconds
                .Then().TheRest().DelayStartBy(20)
                .Then<TestStartupTask>()
                .Sequence;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<TaskExecutionParameters>));
            Assert.AreEqual(0, result.First(t => t.TaskType == typeof(TestStartupTask)).Delay);
            Assert.AreEqual(20, result.First(t => t.TaskType == typeof(IStartupTask)).Delay);
            Assert.AreEqual(15, result.First(t => t.TaskType == typeof(TaskOmega)).Delay);
            Assert.AreEqual(10000, result.First(t => t.TaskType == typeof(TaskBeta)).Delay);
            Assert.AreEqual(5, result.First(t => t.TaskType == typeof(TaskAlpha)).Delay);
        }
    }
}
