using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catalog.Models
{
    [BsonCollection("Course")]
    public class Course : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId
        {
            get; set;
        }

        public string UserId
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price
        {
            get; set;
        }

        public string Picture
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public Feature Feature
        {
            get; set;
        }

        [BsonIgnore]
        public Category Category
        {
            get; set;
        }
    }
}
