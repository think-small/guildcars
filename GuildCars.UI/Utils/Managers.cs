using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GuildCars.UI.Utils
{
    internal static class Managers
    {
        private static readonly UserManager<ApplicationUser> _userManager;
        private static readonly RoleManager<IdentityRole> _roleManager;
        private static readonly ApplicationDbContext _context;

        static Managers()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        internal static async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        internal static async Task<UserDetails> CreateUserDetailsViewModelFrom(string id)
        {
            return await _userManager.Users.Where(u => u.Id == id).Select(u => new UserDetails
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PreSelectedRoles = from userRole in u.Roles
                                   join role in _roleManager.Roles on userRole.RoleId equals role.Id
                                   select role.Id
            }).FirstOrDefaultAsync();
        }

        internal static async Task<IdentityResult> ChangePasswordFor(string id, string password)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(id);
            return await _userManager.ResetPasswordAsync(id, resetToken, password);
        }
    }
}