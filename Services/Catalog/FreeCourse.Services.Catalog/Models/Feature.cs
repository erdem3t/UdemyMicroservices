
namespace FreeCourse.Services.Catalog.Models
{
    [BsonCollection("Feature")]
    public class Feature : Document
    {
        public int Duration
        {
            get; set;
        }
    }
}
