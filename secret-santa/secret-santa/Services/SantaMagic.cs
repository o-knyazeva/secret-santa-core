﻿using System;
using System.Collections.Generic;
using System.Linq;
using SecretSanta.Data;
using SecretSanta.Models.Entities;

namespace SecretSanta.Services
{
	public class SantaMagic : ISantaMagic
	{
		private const int NoSanta = default(int);

        private readonly ApplicationDbContext _dbContext;

        public SantaMagic(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool MixAndMatch(int groupId, ref Participant[] participants)
		{
			var exclusions = from e in _dbContext.ParDrawRules
				join p in _dbContext.Participants on e.ParticipantId equals p.ParticipantId
				where p.GroupId == groupId
				select new ExclusionSlim
				{
					ParticipantId = p.ParticipantId,
					ExcludedParticipantId = e.ExcludeParticipantId
				};

			return AssignSanta(participants, exclusions.ToArray());

			//var success = ValidateAndDump(dbContext, participants);

			//return success;
		}

		private static bool AssignSanta(Participant[] santas, ExclusionSlim[] exclusions)
		{
			var iterations = 0;

			do
			{
				var seed = Math.Abs((int)DateTime.UtcNow.Ticks);
				var random = new Random(seed);

				foreach (var participant in santas)
					participant.HappyKidId = NoSanta;

				var kids = santas.ToArray();

				Shuffle(kids, random);

				for (var i = 0; i < santas.Length; i++)
				{
					var santa = santas[i];
					var kid = kids[i];

					if (santa.UserId == kid.UserId)
						break;

					var isExcluded = exclusions.Any(e => e.ParticipantId == santa.ParticipantId && e.ExcludedParticipantId == kid.ParticipantId);
					if (isExcluded)
						break;

					santa.HappyKidId = kid.ParticipantId;
				}

				iterations++;

                if (iterations > 10000)
                    return false; //Console.WriteLine("It takes way too long, try restart");

			} while (santas.Count(p => p.HappyKidId == NoSanta) > 0);

            return true;
		}

		//public static bool ValidateAndDump(ApplicationDbContext dbContext, IList<Participant> participants)
		//{
		//	Console.WriteLine();

		//	var users = (from u in _users
		//				 join p in participants on u.UserId equals p.ParticipantId
		//				 select new
		//				 {
		//					 p.UserId,
		//					 u.Name,
		//					 p.HappyKidId
		//				 }).ToArray();

		//	var results = (from u in users
		//				   join p in _users on u.HappyKidId equals p.UserId
		//				   select new
		//				   {
		//					   SantaName = u.Name,
		//					   HappyKidName = p.Name
		//				   }).ToArray();

		//	foreach (var result in results)
		//		Console.WriteLine("{0} => {1}", result.SantaName, result.HappyKidName);

		//	Console.WriteLine();

		//	var anyIssues = false;

		//	foreach (var santaWithoutKid in users.Where(u => u.HappyKidId == NoSanta))
		//	{
		//		anyIssues = true;
		//		Console.WriteLine("Not santa {0}", santaWithoutKid.Name);
		//	}

		//	var doubleGifts = new Dictionary<string, int>();
		//	foreach (var result in results)
		//	{
		//		var key = result.HappyKidName;
		//		if (doubleGifts.ContainsKey(key))
		//			doubleGifts[key]++;
		//		else
		//			doubleGifts.Add(key, 1);
		//	}

		//	foreach (var doubleGift in doubleGifts)
		//	{
		//		if (doubleGift.Value > 1)
		//		{
		//			anyIssues = true;
		//			var santas = String.Join(", ", results.Where(r => r.HappyKidName == doubleGift.Key).Select(r => r.SantaName));
		//			Console.WriteLine("Double gift from {0} to {1}", santas, doubleGift.Key);
		//		}
		//	}

		//	if (!anyIssues)
		//		Console.WriteLine("Success!");

		//	return !anyIssues;
		//}

		public static void Shuffle<T>(IList<T> list, Random random)
		{
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = random.Next(n + 1);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public sealed class ExclusionSlim
		{
			public int ParticipantId { get; set; }
			public int ExcludedParticipantId { get; set; }
		}
	}
}