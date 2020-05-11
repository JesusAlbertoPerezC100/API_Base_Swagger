using Collateral.SupplierPortal.Apis.Core.Dto;
using Collateral.SupplierPortal.Apis.Core.Interfaces.UserStories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IDemoUserStory userStory;

        public DemoController(IDemoUserStory userStory)
        {
            this.userStory = userStory;
        }

        [HttpPost]
        [Route("AddClient")]
        [ProducesResponseType(200, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> AddClient()
        {
            var result = await userStory.RunDemo().ConfigureAwait(false);
            return Ok(result);
        }
    }
}