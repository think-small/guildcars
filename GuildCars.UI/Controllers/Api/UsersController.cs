using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuildCars.UI.Controllers.Api
{
    public class UsersController : ApiController
    {
        [Route("api/user/{id}")]
        public async Task<IHttpActionResult> GetUserData(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByIdAsync(id);

            return Json(user);
        }
    }
}
