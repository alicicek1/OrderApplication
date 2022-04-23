using MongoDB.Driver;
using OrderApplication.Core.Business.Abstraction.Mongo;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Model.Document;
using OrderApplication.Core.Model.Util.Response;
using System.Linq.Expressions;

namespace OrderApplication.Core.Business.Concretion.Mongo
{
    public class MongoService<TDocument> : IMongoService<TDocument> where TDocument : IDocument
    {
        private readonly IMongoRepository<TDocument> mongoRepository;

        #region Ctor

        public MongoService(IMongoRepository<TDocument> _mongoRepository)
        {
            this.mongoRepository = _mongoRepository;
        }

        #endregion

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return mongoRepository.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return mongoRepository.FilterBy(filterExpression);
        }

        public virtual IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression)
        {
            return mongoRepository.FilterBy(filterExpression);
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return mongoRepository.FilterBy(filterExpression, projectionExpression);
        }

        public virtual IEnumerable<TDocument> GetAllWithoutFilter()
        {
            return mongoRepository.FilterBy(a => true);
        }

        public virtual IEnumerable<TDocument> GetWithoutFilterPagination(int skipSize, int takeSize)
        {
            return mongoRepository.FilterByPagination(a => true, skipSize, takeSize);
        }

        public virtual TDocument FindByObjectId(string id)
        {
            return mongoRepository.FindByObjectId(id);
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return mongoRepository.FindOne(filterExpression);
        }




        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() => mongoRepository.FindByIdAsync(id));
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => mongoRepository.FindOneAsync(filterExpression));
        }

        public virtual Task InsertManyAsync(ICollection<TDocument> documents)
        {
            return Task.Run(() => mongoRepository.InsertManyAsync(documents));
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => mongoRepository.InsertOneAsync(document));
        }

        public virtual Task ReplaceOneAsync(TDocument document)
        {
            return Task.Run(() => mongoRepository.ReplaceOneAsync(document));
        }

        public virtual Task DeleteByIdAsync(string id)
        {
            return Task.Run(() => mongoRepository.DeleteByIdAsync(id));
        }

        public virtual Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => mongoRepository.DeleteManyAsync(filterExpression));
        }

        public virtual Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => mongoRepository.DeleteOneAsync(filterExpression));
        }



        public virtual DataResponse DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            mongoRepository.DeleteMany(filterExpression);
            return new DataResponse();
        }

        public virtual DataResponse DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            mongoRepository.DeleteOne(filterExpression);
            return new DataResponse();
        }

        public virtual DataResponse InsertMany(ICollection<TDocument> documents)
        {
            mongoRepository.InsertMany(documents);
            return new DataResponse();
        }

        public virtual DataResponse ReplaceOne(TDocument document)
        {
            mongoRepository.ReplaceOneAsync(document);
            return new DataResponse();
        }

        public virtual DataResponse InsertOne(TDocument document)
        {
            mongoRepository.InsertOneAsync(document);
            return new DataResponse();
        }

        public DataResponse DeleteOne(string id)
        {
            mongoRepository.DeleteByIdAsync(id);
            return new DataResponse();
        }


        public virtual long GetCount()
        {
            return mongoRepository.GetCount();
        }

        public virtual long GetCountBy(FilterDefinition<TDocument> filterExpression)
        {
            return mongoRepository.GetCountBy(filterExpression);
        }

    }
}
