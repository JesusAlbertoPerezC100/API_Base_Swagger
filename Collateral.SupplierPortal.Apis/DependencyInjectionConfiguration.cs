using Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories;
using Collateral.SupplierPortal.Apis.Core.Interfaces.UserStories;
using Collateral.SupplierPortal.Apis.Core.UserStories;
using Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Collateral.SupplierPortal.Apis
{
    public static class DependencyInjectionConfiguration
    {
        public static void Load(this IServiceCollection services)
        {
            if (services == null)
                return;

            services.AddTransient<IDatabaseUnitOfWork, DatabaseUnitOfWorkBase<CollateralContext>>();
            services.AddTransient<IDemoUserStory, DemoUserStory>();
        }
    }
}