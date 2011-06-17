﻿using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Ninject
{
    public static class BootstrapperNinjectHelper
    {
        public static IBootstrapperContainerExtensionOptions Ninject(this BootstrapperExtensions extensions)
        {
            var extension = new NinjectExtension();
            extensions.Extension(extension);
            return extension.Options;
        }
    }
}