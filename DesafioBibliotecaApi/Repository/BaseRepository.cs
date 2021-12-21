using DesafioBibliotecaApi.Entidades;
using DesafioBibliotecaApi.Entities;
using System.Collections.Generic;

namespace DesafioBibliotecaApi.Repository
{
    public class BaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey> where TKey : notnull
    {
        protected readonly Dictionary<TKey, TEntity> _store;

        public BaseRepository()
        {
            _store ??= new Dictionary<TKey, TEntity>();
        }

        public bool Create(TEntity entity)
        {
            return _store.TryAdd(entity.Id, entity);

        }

        public bool Update(TEntity entity)
        {
            if(_store.TryGetValue(entity.Id, out var old))
            {
                _store[entity.Id] = entity;
                return true;
            }

            return false;
        }

        public bool Delete(TEntity entity)
        {
            if (_store.TryGetValue(entity.Id, out var old))
            {
                _store.Remove(entity.Id);
                return true;
            }

            return false;

        }

        public TEntity Get(TKey key)
        {
            if(_store.TryGetValue(key, out var old))
            {
                return old;
            }

            return null;
                          
        }

    }
}
