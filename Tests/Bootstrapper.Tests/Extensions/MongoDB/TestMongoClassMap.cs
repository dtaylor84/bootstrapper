using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bootstrap.Tests.Extensions.MongoDB
{
    public class TestMongoClassMap: BsonClassMap<TestMongo>
    {
        public TestMongoClassMap()
        {
            var idMember = MapField(p => p.Id)
                .SetRepresentation(BsonType.ObjectId)
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            SetIdMember(idMember);

            MapField(p => p.Name);
        }
    }

    public class TestMongo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
