using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models.Entities
{
    public class DrawRule
    {
        public int DrawRuleId { get; set; }
        public int ParticipantId { get; set; }
        public int ExcludeParticipantId { get; set; }
    }
}
