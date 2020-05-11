using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories
{
    public interface IDatabaseUnitOfWork
    {
        IBaseRepository<TEntity> Repository<TEntity>()
            where TEntity : class;

        IBaseRepositoryFunction<TEntity, TP> RepositoryFunctios<TEntity, TP>()
            where TEntity : class;

        Task ExecProcedure(string procedure, params object[] parameters);

        bool MultipleWorkThreads { get; set; }

        Task<bool> Save();

        //
        Task Test();
    }

    public interface IDatabaseUnitOfWork<TEntity> : IDatabaseUnitOfWork
    {
    }
}