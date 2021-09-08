using GuildCars.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace GuildCars.UI.Models
{
    public class EditVehicleViewModel
    {
        public Vehicle Vehicle { get; set; }
        public Dictionary<Make, IList<Model>> MakesAndModels { get; set; }
        public IEnumerable<SelectListItem> TransmissionTypes { get; set; }
        public IEnumerable<SelectListItem> BodyStyles { get; set; }
        public IEnumerable<SelectListItem> Details { get; set; }
        public int[] SelectedDetailIds { get; set; }
        public List<string> ImagesToKeep { get; set; }
        public HttpPostedFileBase[] ImageUploads { get; set; }
    }
}