using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models.AdminViewModels
{
    public class AdminViewModel
    {
        public int GroupId { get; set; }
        public List<SelectListItem> Groups { get; set; } = new List<SelectListItem>();
        public string ResultMessage { get; set; }

    }
}
