using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoRepair.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class SendMailController : Controller
    {
        private readonly IMailHelper _mailHelper;

        public SendMailController(
            IMailHelper mailHelper)
        {
            _mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(SendEmailViewModel model)
        {
            Response response = _mailHelper.SendEmail(model.SendGmail, model.Username, model.Message);

            return View();
        }
    }
}
