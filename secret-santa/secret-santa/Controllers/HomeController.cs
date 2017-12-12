using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Models;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using SecretSanta.Models.HomeViewModels;
using Microsoft.AspNetCore.Identity;
using System.Net.Mime;
using System.Net;

namespace SecretSanta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            var userId = _userManager.GetUserId(HttpContext.User);

            var userGroups = _dbContext.Groups
                .Where(t => t.Participants.Select(p => p.UserId).Contains(userId));

            if (userGroups.Any())
            {
                model.Groups = userGroups.Select(t => new SelectListItem() { Value = t.GroupId.ToString(), Text = t.Name }).ToList(); //TODO!

                var selectedGroup = userGroups
                    .Include(t => t.Participants)
                    .ThenInclude(u => u.User)
                    .Last();

                model.GroupId = selectedGroup.GroupId;

                if (selectedGroup != null)
                {
                    model.GroupId = selectedGroup.GroupId;
                    model.Participants = selectedGroup.Participants;

                    var currentParticipant = selectedGroup.Participants.FirstOrDefault(t => t.UserId == userId);
                    model.CurrentParticipantId = currentParticipant?.ParticipantId;
                    model.HappyKidId = currentParticipant?.HappyKidId;

                    model.LetterText = currentParticipant?.LetterText;
                }
            }
            
            
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitLetter(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var participant = _dbContext.Participants.FirstOrDefault(t => t.ParticipantId == model.CurrentParticipantId);
                if (participant != null)
                {
                    participant.LetterText = model.LetterText;
                }

                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
