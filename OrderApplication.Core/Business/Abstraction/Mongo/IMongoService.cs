using MongoDB.Driver;
using OrderApplication.Core.Model.Document;
using OrderApplication.Core.Model.Util.Response;
using System.Linq.Expressions;

namespace OrderApplication.Core.Business.Abstraction.Mongo
{
    public interface IMongoService<TDocument> where TDocument : IDocument
    {
        IEnumerable<TDocument> GetAllWithoutFilter();
        IEnumerable<TDocument> GetWithoutFilterPagination(int skipSize, int takeSize);
        IQueryable<TDocument> AsQueryable();
        IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
        IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression);
        IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression);
        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
        TDocument FindByObjectId(string id);


        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindByIdAsync(string id);
        Task InsertOneAsync(TDocument document);
        Task InsertManyAsync(ICollection<TDocument> documents);
        Task ReplaceOneAsync(TDocument document);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteByIdAsync(string id);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);



        DataResponse InsertMany(ICollection<TDocument> documents);
        DataResponse ReplaceOne(TDocument document);
        DataResponse InsertOne(TDocument document);
        DataResponse DeleteOne(Expression<Func<TDocument, bool>> filterExpression);
        DataResponse DeleteOne(string id);
        DataResponse DeleteMany(Expression<Func<TDocument, bool>> filterExpression);


        long GetCount();
        long GetCountBy(FilterDefinition<TDocument> filterExpression);
    }
}
