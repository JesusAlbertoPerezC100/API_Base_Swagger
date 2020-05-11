using Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework
{
    public class BaseRepositoryFunction<T, TP> : IBaseRepositoryFunction<T, TP> where T : class
    {
        private const string _select = "select * from ";
        private readonly DbContext _ctx;

        public BaseRepositoryFunction(DbContext context)
        {
            _ctx = context;
        }

        public async Task<IEnumerable<T>> ExecFunctionFindBy(TP param)
        {
            var cmd = $"{_select}{GetFunctionName(typeof(T))}" + "({0})";
            var data = await _ctx.Set<T>().FromSqlRaw<T>(cmd, param).ToListAsync().ConfigureAwait(false);
            return data;
        }

        public async Task<IEnumerable<T>> ExecFunctionFindBy(params object[] parameters)
        {
            var cmd = $"{_select}{GetFunctionName(typeof(T))}";

            var stringbuilder = new StringBuilder();
            stringbuilder.Append("(");

            for (int i = 0; i < parameters.Length; i++)
                stringbuilder.Append(string.Concat("{", i.ToString(CultureInfo.CurrentCulture), "},"));

            cmd += stringbuilder.ToString().Substring(0, (stringbuilder.ToString().Length - 1)) + ")";
            var data = await _ctx.Set<T>().FromSqlRaw<T>(cmd, parameters).ToListAsync().ConfigureAwait(false);
            return data;
        }

        private static string GetFunctionName(Type type)
        {
            var att = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute), false);
            return att?.Name;
        }
    }
}