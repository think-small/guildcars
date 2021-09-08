using GuildCars.Services.InventoryService;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuildCars.UI.Controllers.Api
{
    public class InventoryController : ApiController
    {
        private readonly IInventoryService _inventory;
        public InventoryController(IInventoryService inventory)
        {
            _inventory = inventory;
        }

        [Route("api/inventory/{id}")]
        public async Task<IHttpActionResult> GetDetailsFor(int id)
        {
            try
            {
                var vehicle = await _inventory.GetDetailsFor(id);
                return Json(vehicle);
            }
            catch (Exception e)
            {
                return BadRequest($"Unable to retrieve vehicle details for id {id}: {e.Message}");
            }
        }
    }
}
