using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models.Entities
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public string LetterText { get; set; }

        public Group Group { get; set; }
        public ApplicationUser User { get; set; }
    }
}
