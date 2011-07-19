using System.Reflection;

namespace Bootstrap.NHibernate.Wcf
{
    public class Initializer
    {
        public void Initialize()
        {
            var mapsAssembly = Assembly.GetAssembly(typeof(NoteToNoteDataContractMapCreator));
            var diemAssembly = Assembly.GetAssembly(typeof (IIngestionErrorController));
            Bootstrapper.
                Including.
                    Assembly(mapsAssembly).
                    AndAssembly(diemAssembly).
                With.StructureMap().
                    And.AutoMapper().
                    And.ServiceLocator().
                    And.StartupTasks().
                Start();
        }
    }
}