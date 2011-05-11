using System.Collections.Generic;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Extensions
{
    public class BootstrapperExtensions
    {
        private readonly List<IBootstrapperExtension> extensions = new List<IBootstrapperExtension>();

        public BootstrapperExtensions And { get { return this; } }

        public void ClearExtensions()
        {
            extensions.Clear();
        }

        public BootstrapperExtensions Extension(IBootstrapperExtension extension)
        {
            extensions.Add(extension);
            if(extension is IBootstrapperContainerExtension) 
                Bootstrapper.ContainerExtension = extension as IBootstrapperContainerExtension;
            return this;
        }

        public IList<IBootstrapperExtension> GetExtensions()
        {
            return extensions.AsReadOnly();
        }

        public void Start()
        {
            Bootstrapper.Start();
        }
    }
}
