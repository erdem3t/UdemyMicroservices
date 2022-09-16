using FreeCourse.Services.Catalog.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.RepositoryContract
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression);

        Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProtected> FilterBy<TProtected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProtected>> projectionExpression);


        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(string id);

        Task<TDocument> FindByIdAsync(ObjectId id);

        void InsertOne(TDocument document);

        Task InsertOneAsync(TDocument document);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(ICollection<TDocument> documents);

        void ReplaceOne(TDocument document);

        Task ReplaceOneAsync(TDocument document);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        void DeleteById(string Id);

        Task DeleteByIdAsync(string Id);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    }
}
