using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bootstrapper.Tests.Other
{
    public static class ExceptionAssert
    {
        public static T Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                return ex;
            }
            Assert.Fail("Exception of type {0} should be thrown.", typeof(T));

            //  The compiler doesn't know that Assert.Fail
            //  will always throw an exception
            return null;
        }
    }
}
