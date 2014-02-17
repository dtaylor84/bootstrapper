using System;
using System.Text;
using Bootstrap.Extensions;

namespace Bootstrapper.MongoDB
{
    public static class MongoConvenienceExtensions
    {
        public static BootstrapperExtensions MongoDB(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new MongoExtension(Bootstrap.Bootstrapper.RegistrationHelper));
        }

    }
}
