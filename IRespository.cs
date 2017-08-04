using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    public interface IRespository<TEntity> where TEntity:class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetSingleById(int Id);
        void Insert(TEntity obj);
        void Update(TEntity obj);
        void Delete(int Id);
        TEntity Find(int Id);
        void Save();
    }
}
