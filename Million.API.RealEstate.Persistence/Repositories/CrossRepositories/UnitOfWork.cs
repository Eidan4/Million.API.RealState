using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;

namespace Million.API.RealEstate.Persistence.Repositories.CrossRepositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase _database;
        private readonly Dictionary<Type, object> _repositories;

        private IPropertyRepository? _propertyRepository;

        private IPropertyTraceRepository? _propertyTraceRepository;

        public UnitOfWork(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var collectionName = typeof(T).Name;
                var repository = new GenericRepository<T>(_database, collectionName);
                _repositories.Add(typeof(T), repository);
            }
            return (IGenericRepository<T>)_repositories[typeof(T)];
        }

        public IPropertyRepository PropertyRepository
        {
            get
            {
                if (_propertyRepository == null)
                {
                    _propertyRepository = new PropertyRepository(_database);
                }
                return _propertyRepository;
            }
        }

        public IPropertyTraceRepository PropertyTraceRepository
        {
            get
            {
                if (_propertyTraceRepository == null)
                {
                    _propertyTraceRepository = new PropertyTraceRepository(_database);
                }
                return _propertyTraceRepository;
            }
        }

        public void Dispose()
        {
            // No se requiere liberar el contexto de MongoDB explícitamente.
        }
    }
}