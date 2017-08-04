using ClassLibrary2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace RepositoryPattern
{
    public class Repository<T> : IRespository<T> where T : class
    {
        private DbContext context;
        private DbSet<T> dbSet;

        public Repository()
        {
            this.context = new OrderEntities();
            dbSet = context.Set<T>();
        }
        public Repository(DbContext c)
        {
            this.context = c;
            dbSet = context.Set<T>();
        }


        public void Delete(int Id)
        {
            T getObjById = dbSet.Find(Id);
            dbSet.Remove(getObjById);
        }

        public T Find(int Id)
        {
           T entity=dbSet.Find(Id);
            return entity;
            
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetSingleById(int Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T obj)
        {
            dbSet.Add(obj);
            
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;
        }
    }
}
