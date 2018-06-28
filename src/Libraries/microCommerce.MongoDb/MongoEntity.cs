using microCommerce.Domain;
using MongoDB.Bson;

namespace microCommerce.MongoDb
{
    public abstract class MongoEntity : BaseEntityTypeId<ObjectId>
    {
        public override ObjectId Id { get; set; }
    }
}