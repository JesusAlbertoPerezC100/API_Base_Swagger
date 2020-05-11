using Collateral.SupplierPortal.Apis.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories
{
    public interface IBaseRepository<T> : IBaseRepository
    where T : class
    {
        Task<bool> Add(T entity);

        Task<IEnumerable<T>> GetAll();

        Task<PagedResult<T>> GetAll(int page, int pageSize);

        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);

        Task<PagedResult<T>> GetAll(int page, int pageSize, Expression<Func<T, bool>> predicate);

        Task<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<bool> Update(T entity);

        Task<bool> Delete(Expression<Func<T, bool>> identity);

        Task<bool> Delete(T entity);

        IQueryable<T> UsingSelect();
    }

    public interface IBaseRepository
    {
        Task<bool> Save();

        void ChangeMultipleWorkThreads(bool multiple);
    }

    public interface IBaseRepositoryFunction<T, TP>
        where T : class
    {
        Task<IEnumerable<T>> ExecFunctionFindBy(TP param);

        Task<IEnumerable<T>> ExecFunctionFindBy(params object[] parameters);
    }
}