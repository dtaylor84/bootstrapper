namespace Bootstrap.NHibernate.Wcf
{
    [ServiceErrorBehavior(typeof(HttpErrorHandler))]
    [SessionPerCallServiceBehavior]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ErrorManagementService : IErrorManagementService
    {

   }
}