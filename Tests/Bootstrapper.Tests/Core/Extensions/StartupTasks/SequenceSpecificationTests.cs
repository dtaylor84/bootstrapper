using System;
using System.Collections.Generic;
using Bootstrap.Extensions.StartupTasks;
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
            Assert.IsInstanceOfType(result, typeof(List<Type>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldAddATaskToTheTopOfTheSequence()
        {
            //Arrange
            var spec = new SequenceSpecification();
            spec.Sequence.Add(typeof(TaskBeta));
            spec.Sequence.Add(typeof(TaskOmega));

            //Act
            spec.First<TaskAlpha>();
            var result = spec.Sequence;

            //Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(typeof(TaskAlpha), result[0]);
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
            spec.Sequence.Add(typeof(TaskAlpha));
            spec.Sequence.Add(typeof(TaskBeta));

            //Act
            spec.Then<TaskOmega>();
            var result = spec.Sequence;

            //Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreSame(typeof(TaskOmega), result[result.Count-1]);
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

    }
}
