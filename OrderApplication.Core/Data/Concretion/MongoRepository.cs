using MongoDB.Bson;
using MongoDB.Driver;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Model.Document;
using OrderApplication.Core.Model.Util.AppSettings;
using OrderApplication.Core.Model.Util.Attribute;
using System.Linq.Expressions;

namespace OrderApplication.Core.Data.Concretion
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
     where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        #region ctor

        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        #endregion


        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression, new FindOptions { AllowDiskUse = true }).ToEnumerable();
        }

        public virtual IEnumerable<TDocument> FilterBy(FilterDefinition<TDocument> filterExpression)
        {
            return _collection.Find(filterExpression, new FindOptions { AllowDiskUse = true }).ToEnumerable();
        }

        public virtual IEnumerable<TDocument> FilterByPagination(Expression<Func<TDocument, bool>> filterExpression, int skipSize, int takeSize)
        {
            return _collection.Find(filterExpression, new FindOptions { AllowDiskUse = true }).Skip(skipSize).Limit(takeSize).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual TDocument FindByObjectId(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }


        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public virtual TDocument InsertOne(TDocument document)
        {
            do
            {
                document.Id = ObjectId.GenerateNewId().ToString();
            } while (this.FindByObjectId(document.Id.ToString()) != null);


            _collection.InsertOne(document);
            return document;
        }

        TDocument IMongoRepository<TDocument>.ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            this._collection.FindOneAndReplace(filter, document);
            return document;
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }


        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }


        public virtual long GetCount()
        {
            return _collection.CountDocuments(x => true);
        }

        public virtual long GetCountBy(FilterDefinition<TDocument> filterExpression)
        {
            return _collection.CountDocuments(filterExpression);
        }

    }
}
