using System;
using System.Collections.Generic;
using System.Text;

namespace Level1_Sprint1
{
    public class DataStore<T> where T : class, IEntity
    {
        private readonly List<T> Entities = new List<T>();
        public void Add(T entity)
        {
            Entities.Add(entity);
        }
        public void Update(T entity)
        {
            var existingEntity = Entities.Find(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                Entities.Remove(existingEntity);
                Entities.Add(entity);
            }
        }
        public void Remove(T entity)
        {
            Entities.Remove(entity);
        }
        public T GetById(int id)
        {
            foreach (var entity in Entities)
            {
                if (entity.Id == id)
                {
                    return entity;
                }
            }
            return null;
        }
        public List<T> GetAll()
        {
            return Entities;
        }

    }
}
