using Microsoft.Practices.ServiceLocation;

namespace Bootstrapper.Tests.Extensions.Containers
{
    public class TestContainerExtension: BootstrapperContainerExtension
    {
        private object container;
        private IServiceLocator locator;

        public TestContainerExtension(object theContainer)
        {
            container = theContainer;
        }
       
        public void SetTestServiceLocator(IServiceLocator theLocator)
        {
            locator = theLocator;
        }

        protected override void InitializeServiceLocator()
        {
            if(locator != null)
                ServiceLocator.SetLocatorProvider(() => locator);
            SetContainer(container);
        }

        protected override void ResetContainer()
        {
            container = null;
            SetContainer(container);
        }

        protected override void InitializeContainer() { }
        protected override void RegisterImplementationsOfIRegistration() { }
        protected override void InvokeRegisterForImplementationsOfIRegistration() { }

    }
}
