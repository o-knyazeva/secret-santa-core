using SecretSanta.Models.Entities;

namespace SecretSanta.Services
{
	public interface ISantaMagic
	{
		bool MixAndMatch(int groupId, ref Participant[] participants);
	}
}