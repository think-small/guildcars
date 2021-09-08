using GuildCars.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuildCars.UI.Models
{
    public class UserDetails
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public IEnumerable<IdentityRole> Roles{ get; set; }
        public IEnumerable<string> PreSelectedRoles { get; set; }
        [Display(Name = "Roles")]
        public IEnumerable<string> SelectedRoles { get; set; }
        [Required]
        [Display(Name = "New Password")]
        public string NewPassword  { get; set; }
        [Required]
        [MinLength(8)]
        [Compare("NewPassword")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public IEnumerable<Vehicle> VehiclesOwned { get; set; }
    }
}