using System;

namespace Bootstrapper
{
    public class BootstrapperExtension : IBootstrapperExtension
    {
        protected void SetContainer(object container)
        {
            Bootstrapper.Container = container;
        }

        public virtual void Run() {}
        public virtual void Reset() {}
    }
}
