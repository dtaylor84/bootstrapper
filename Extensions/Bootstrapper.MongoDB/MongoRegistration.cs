using System;
using Bootstrap.Extensions.Containers;
using MongoDB.Bson.Serialization;

namespace Bootstrapper.MongoDB
{
    public class MongoRegistration : IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension containerExtension)
        {
            containerExtension.RegisterAll<BsonClassMap>();
        }
    }
}