using Collateral.SupplierPortal.Apis.Core.Dto;
using Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories;
using Collateral.SupplierPortal.Apis.Core.Interfaces.UserStories;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Core.UserStories
{
    public class DemoUserStory : IDemoUserStory
    {
        private readonly IDatabaseUnitOfWork databaseUnitOfWork;

        public DemoUserStory(IDatabaseUnitOfWork databaseUnitOfWork)
        {
            this.databaseUnitOfWork = databaseUnitOfWork;
        }

        public async Task<ResponseMessage> RunDemo()
        {
            return await Task.Run(() => new ResponseMessage()).ConfigureAwait(false);
        }
    }
}