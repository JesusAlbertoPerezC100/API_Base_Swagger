using Microsoft.EntityFrameworkCore;

namespace Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework
{
    public class CollateralContext : DbContext
    {
        public CollateralContext(DbContextOptions<CollateralContext> options) : base(options)
        {
        }
    }
}