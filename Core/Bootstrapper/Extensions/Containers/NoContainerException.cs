using System;

namespace Bootstrap.Extensions.Containers
{
    public class NoContainerException: ApplicationException
    {
        public static string DefaultMessage = "Unable to continue. The container has not been initialized.";

        public NoContainerException(): base(DefaultMessage){}
        public NoContainerException(string message) : base(message){}
    }
}
