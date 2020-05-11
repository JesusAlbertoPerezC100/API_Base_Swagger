using Collateral.SupplierPortal.Apis.Core.Dto;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Core.Interfaces.UserStories
{
    public interface IDemoUserStory
    {
        Task<ResponseMessage> RunDemo();
    }
}