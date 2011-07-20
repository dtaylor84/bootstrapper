using Bootstrap.Extensions;

namespace Bootstrap.NHibernate.Wcf
{
    public static class BootstrapperNHibernateWcfHelper
    {
        public static BootstrapperExtensions NHibernateWcf(this BootstrapperExtensions extensions)
        {
            return extensions.Extension(new NHibernateWcfExtension());
        }
    }
}
