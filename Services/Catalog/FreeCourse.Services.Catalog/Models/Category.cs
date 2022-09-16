
namespace FreeCourse.Services.Catalog.Models
{
    [BsonCollection("Category")]
    public class Category : Document
    {
        public string Name
        {
            get; set;
        }
    }
}
