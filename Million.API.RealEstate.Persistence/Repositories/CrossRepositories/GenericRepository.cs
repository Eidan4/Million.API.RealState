using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.API.RealEstate.Persistence.Repositories.CrossRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<T> GetAsync(string id)
        {
            return await _collection.Find(x => x.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}