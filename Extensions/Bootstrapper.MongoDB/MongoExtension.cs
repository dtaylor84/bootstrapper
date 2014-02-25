using System.Linq;
using Bootstrap;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using MongoDB.Bson.Serialization;

namespace Bootstrapper.MongoDB
{
    public class MongoExtension : IBootstrapperExtension
    {
        private readonly IRegistrationHelper registrationHelper;

        public MongoExtension(IRegistrationHelper registrationHelper)
        {
            Bootstrap.Bootstrapper.Excluding.Assembly("MongoDB.Bson");
            Bootstrap.Bootstrapper.Excluding.Assembly("MongoDB.Driver");
            this.registrationHelper = registrationHelper;
        }

        public void Run()
        {
            var profiles = Bootstrap.Bootstrapper.ContainerExtension != null && Bootstrap.Bootstrapper.Container != null
                ? Bootstrap.Bootstrapper.ContainerExtension.ResolveAll<BsonClassMap>().ToList()
                : registrationHelper.GetInstancesOfTypesImplementing<BsonClassMap>();

            profiles
                .Where(b => !BsonClassMap.IsClassMapRegistered(b.ClassType))
                .ForEach(BsonClassMap.RegisterClassMap);
        }

        public void Reset() {}
    }
}