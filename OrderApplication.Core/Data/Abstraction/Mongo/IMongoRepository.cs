using MongoDB.Driver;
using OrderApplication.Core.Model.Document;
using System.Linq.Expressions;

namespace OrderApplication.Core.Data.Abstraction.Mongo
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
        IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression);
        IEnumerable<TDocument> FilterByPagination(Expression<Func<TDocument, bool>> filterExpression, int skipSize, int takeSize);
        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
        TDocument FindByObjectId(string id);


        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindByIdAsync(string id);
        Task InsertOneAsync(TDocument document);
        Task InsertManyAsync(ICollection<TDocument> documents);
        Task ReplaceOneAsync(TDocument document);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
        void DeleteById(string id);


        TDocument InsertOne(TDocument document);
        TDocument ReplaceOne(TDocument document);
        void InsertMany(ICollection<TDocument> documents);
        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        long GetCount();
        long GetCountBy(FilterDefinition<TDocument> filterExpression);
    }
}
