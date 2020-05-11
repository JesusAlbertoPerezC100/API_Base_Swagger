using Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework
{
    public class DatabaseUnitOfWorkBase<TDBContext> : IDatabaseUnitOfWork<TDBContext>
       where TDBContext : DbContext
    {
        private readonly IDictionary<string, IBaseRepository> Repositories;

        public DatabaseUnitOfWorkBase(TDBContext databaseContext)
        {
            DatabaseContext = databaseContext;
            Repositories = new Dictionary<string, IBaseRepository>();
        }

        public virtual TDBContext DatabaseContext { get; private set; }

        private bool _MultipleWorkThreads = false;

        public bool MultipleWorkThreads
        {
            get { return _MultipleWorkThreads; }
            set
            {
                _MultipleWorkThreads = value;
                if (Repositories != null && Repositories.Any())
                    foreach (var item in Repositories.Keys)
                        Repositories[item].ChangeMultipleWorkThreads(value);
            }
        }

        public virtual async Task<bool> Save()
        {
            _ = await DatabaseContext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public IBaseRepository<TEntity> Repository<TEntity>()
            where TEntity : class
        {
            var name = typeof(TEntity).Name;
            if (Repositories.ContainsKey(name))
                return Repositories[name] as BaseRepository<TEntity>;
            else
            {
                Repositories.Add(name, new BaseRepository<TEntity>(DatabaseContext, DatabaseContext.Set<TEntity>(), MultipleWorkThreads));
                return Repositories[name] as BaseRepository<TEntity>;
            }
        }

        public IBaseRepositoryFunction<TEntity, TP> RepositoryFunctios<TEntity, TP>() where TEntity : class
        {
            return new BaseRepositoryFunction<TEntity, TP>(DatabaseContext);
        }

        private const string _call = "CALL ";

        public async Task ExecProcedure(string procedure, params object[] parameters)
        {
            var cmd = $"{_call}{procedure}";

            var stringbuilder = new StringBuilder();
            stringbuilder.Append("(");

            for (int i = 0; i < parameters.Length; i++)
                stringbuilder.Append(string.Concat("{", i.ToString(CultureInfo.CurrentCulture), "},"));

            cmd += stringbuilder.ToString().Substring(0, (stringbuilder.ToString().Length - 1)) + ");";

            _ = await DatabaseContext.Database.ExecuteSqlRawAsync(cmd, parameters).ConfigureAwait(false);
        }

        public async Task Test()
        {
            _ = await DatabaseContext.Database.ExecuteSqlRawAsync("SELECT NOW();").ConfigureAwait(false);
        }
    }
}