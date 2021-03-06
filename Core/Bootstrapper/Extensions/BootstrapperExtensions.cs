﻿using System.Collections.Generic;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Extensions
{
    public class BootstrapperExtensions: BootstrapperOption
    {
        private readonly List<IBootstrapperExtension> extensions = new List<IBootstrapperExtension>();

        public new BootstrapperExtensions And { get { return this; } }
        public IList<IBootstrapperExtension> GetExtensions() {return extensions.AsReadOnly();}
        public void ClearExtensions() {extensions.Clear();}

        public BootstrapperExtensions Extension(IBootstrapperExtension extension)
        {
            extensions.Add(extension);
            if(extension is IBootstrapperContainerExtension) 
                Bootstrapper.ContainerExtension = extension as IBootstrapperContainerExtension;
            return this;
        }
    }
}
