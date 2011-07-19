namespace Bootstrap.NHibernate.Wcf
{
    public class NHibernateContextManager : IExtension<InstanceContext>
    {
        public ISession Session { get; set; }

        public void Attach(InstanceContext owner)
        {
            //We have been attached to the Current operation context from the
            // ServiceInstanceProvider
        }

        public void Detach(InstanceContext owner)
        {
            if (Session == null) return;
            Session.Flush();
            Session.Close();
            Session.Dispose();
        }
    }
}