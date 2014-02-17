using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;
using MongoDB.Bson.Serialization;

namespace Bootstrapper.MongoDB
{
    public class MongoExtension : IBootstrapperExtension
    {
        private readonly IRegistrationHelper _registrationHelper;

        public MongoExtension(IRegistrationHelper registrationHelper)
        {
            Bootstrap.Bootstrapper.Excluding.Assembly("MongoDB.Bson");
            Bootstrap.Bootstrapper.Excluding.Assembly("MongoDB.Driver");
            _registrationHelper = registrationHelper;
        }

        public void Run()
        {
            List<BsonClassMap> profiles;

            if (Bootstrap.Bootstrapper.ContainerExtension != null && Bootstrap.Bootstrapper.Container != null)
                profiles = Bootstrap.Bootstrapper.ContainerExtension.ResolveAll<BsonClassMap>().ToList();
            else
                profiles = _registrationHelper.GetInstancesOfTypesImplementing<BsonClassMap>();

            foreach (var bsonClassMap in profiles)
            {
                if (BsonClassMap.IsClassMapRegistered(bsonClassMap.ClassType))
                    continue;

                BsonClassMap.RegisterClassMap(bsonClassMap);
            }

        }

        public void Reset()
        {
        }


    }
}