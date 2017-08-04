using ClassLibrary2;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace CodeMetier.Implementation
{
    public class GenericImplement<TEntity> where TEntity:class
    {
        private OrderEntities context;
       

       public GenericImplement()
        {
            
            this.context = new OrderEntities();
           
        }

        public void Insert(TEntity obj)
        {
            try
            {
                Repository<TEntity> _Repository = new Repository<TEntity>();
                _Repository.Insert(obj);
                _Repository.Save();
                
            }
            catch (Exception e)
            {

            }
        }

        public void Update(TEntity obj)
        {
            try
            {
                Repository<TEntity> _Repository = new Repository<TEntity>(context);
                _Repository.Update(obj);
                _Repository.Save();

            }
            catch (Exception e)
            {

            }
            
        }

        public TEntity GetSingleById(int Id)
        {
            try
            {
                Id=Int32.Parse(""+Id);
                Repository<TEntity> _Repository = new Repository<TEntity>(context);
                TEntity E= _Repository.GetSingleById(Id);
                _Repository.Save();
                return E;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                Repository<TEntity> _Repository = new Repository<TEntity>();
                IEnumerable<TEntity> List = _Repository.GetAll();
                return List;

            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public TEntity Find(int Id)
        {
            try
            {
                Repository<TEntity> _Repository = new Repository<TEntity>(context);
                TEntity entity = _Repository.Find(Id);
                return entity;

            }
            catch (Exception e)
            {
                return null;
            }

        }

        public Boolean SaveAll()
        {
            try
            {
                Repository<TEntity> _Repository = new Repository<TEntity>(context);
                _Repository.Save();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
