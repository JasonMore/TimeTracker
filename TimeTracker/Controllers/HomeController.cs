using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracker.Service.Models;
using TimeTracker.Service.Services;
using Newtonsoft.Json;
using TimeTracker.Service.ViewModels;

namespace TimeTracker.Controllers
{
    public class HomeController : Controller
    {
        private ITrackerService _trackerService;
        public HomeController(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTracker()
        {
            var user = HttpContext.User.Identity.Name;

            var tracker = _trackerService.GetTracker(user);

            return Json(new { tracker = JsonConvert.SerializeObject(tracker) });
        }

        [HttpPost]
        public ActionResult GetProjects()
        {
            var userName = HttpContext.User.Identity.Name;
            var projects = _trackerService.GetProjects(userName);
            return Json(projects);
        }

        [HttpPost]
        public ActionResult Save(string tracker)
        {
            var viewModel = convert(tracker);

            var userName = HttpContext.User.Identity.Name;
            viewModel.userName = userName;

            var model = _trackerService.Save(viewModel);
            return Json(new { trackerId = model.Id, currentTaskId = model.CurrentTask.Id });
        }

        public TrackerViewModel convert(string tracker)
        {
            return JsonConvert.DeserializeObject<TrackerViewModel>(tracker);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }
    }
}
