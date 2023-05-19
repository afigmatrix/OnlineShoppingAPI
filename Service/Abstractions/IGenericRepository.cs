using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineShoppingAPI.Service.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        Task AddAndCommit(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(int id);
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllTable();
        IEnumerable<T> GetValuesByExpression(Expression<Func<T, bool>> expression);
        Task Commit();
    }
}
