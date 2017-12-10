using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models.Entities
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }

        public List<Participant> Participants { get; set; }
    }
}
