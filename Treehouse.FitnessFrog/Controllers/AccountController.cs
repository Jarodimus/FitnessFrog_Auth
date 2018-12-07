using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.ViewModels;

namespace Treehouse.FitnessFrog.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountRegisterViewModel viewModel)
        {
            var user = new User { UserName = viewModel.Email, Email = viewModel.Email };
            return View();
        }
    }
}
