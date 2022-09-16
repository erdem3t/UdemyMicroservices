using MongoDB.Bson;
using System;

namespace FreeCourse.Services.Catalog.Models
{
    public class Document : IDocument
    {
        public ObjectId Id
        {
            get; set;
        }
        public DateTime CreateTime => Id.CreationTime;
    }
}
