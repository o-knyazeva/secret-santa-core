using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Services;
using SecretSanta.Data;
using SecretSanta.Models.AdminViewModels;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace secret_santa.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISantaMagic _santaMagic;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(ISantaMagic santaMagic, ApplicationDbContext dbContext)
        {
            _santaMagic = santaMagic;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var model = new AdminViewModel();
            model.Groups = _dbContext.Groups.Select(t => new SelectListItem() { Value = t.GroupId.ToString(), Text = t.Name }).ToList();
            model.GroupId = _dbContext.Groups.Last().GroupId;

            return View(model);
        }

        [HttpPost]
        public IActionResult DrawNames(AdminViewModel model)
        {
            var participants = _dbContext.Participants.Where(p => p.GroupId == model.GroupId).ToArray();

            if (_santaMagic.MixAndMatch(model.GroupId, ref participants))
            {
                var stringBuider = new StringBuilder();
                foreach (var participant in participants)
                {
                    var currentParticipant = _dbContext.Participants.Where(t => t.ParticipantId == participant.ParticipantId).FirstOrDefault();
                    var happyKid = _dbContext.Participants.Where(t => t.ParticipantId == currentParticipant.HappyKidId).FirstOrDefault();

                    //stringBuider.AppendLine($"{currentParticipant?.User.UserName} => {happyKid?.User.UserName} <br />");

                    currentParticipant.HappyKidId = happyKid.ParticipantId;
                }
                stringBuider.AppendLine("Done");
                _dbContext.SaveChanges();

                model.ResultMessage = stringBuider.ToString();
            }
            else
            {
                model.ResultMessage = "Error! Try again!";
            }

            model.GroupId = model.GroupId;
            return View("Index", model);
        }
    }
}