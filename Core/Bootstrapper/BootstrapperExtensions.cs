using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap
{
    public class BootstrapperExtensions
    {
        private readonly List<IBootstrapperExtension> extensions = new List<IBootstrapperExtension>();
        private IBootstrapperContainerExtension containerExtension;

        public BootstrapperExtensions With { get { return this; } }

        public void ClearExtensions()
        {
            extensions.Clear();
            containerExtension = null;
        }

        public BootstrapperExtensions Extension(IBootstrapperExtension extension)
        {
            extensions.Add(extension);
            return this;
        }

        public BootstrapperExtensions Container(IBootstrapperContainerExtension theContainerExtension)
        {
            containerExtension = theContainerExtension;
            Extension(theContainerExtension);
            return this;
        }


        public IList<IBootstrapperExtension> GetExtensions()
        {
            return extensions.AsReadOnly();
        }

        public void Start()
        {
            Bootstrapper.StartCallingAssembly = Assembly.GetCallingAssembly();
            Bootstrapper.Start();
        }

        public IBootstrapperContainerExtension GetContainerExtension()
        {
            return containerExtension;
        }

    }
}
