namespace SecretSanta.Models.Entities
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string LetterText { get; set; }

	    public int HappyKidId { get; set; }

		public Group Group { get; set; }
        public ApplicationUser User { get; set; }
    }
}
