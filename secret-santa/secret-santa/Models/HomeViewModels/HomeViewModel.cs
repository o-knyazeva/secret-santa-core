using Microsoft.AspNetCore.Mvc.Rendering;
using SecretSanta.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public int? GroupId { get; set; }
        public List<SelectListItem> Groups { get; set; } = new List<SelectListItem>();
        public List<Participant> Participants { get; set; } = new List<Participant>();
        public int? CurrentParticipantId { get; set; }
        public string LetterText { get; set; }
    }
}
