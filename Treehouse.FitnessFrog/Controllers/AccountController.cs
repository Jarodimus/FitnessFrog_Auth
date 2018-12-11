using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Shared.Security;
using Treehouse.FitnessFrog.ViewModels;

namespace Treehouse.FitnessFrog.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Entries");
        }


        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(AccountSignInViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            SignInStatus result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Entries");
                case SignInStatus.Failure:
                    ModelState.AddModelError("", $"Invalid username and/or password.");
                    return View(viewModel);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", $"You have been locked out after too many invalid login attempts.");
                    return View(viewModel);
                case SignInStatus.RequiresVerification:
                    throw new NotImplementedException("Identity feature not implemented.");
                default:
                    throw new Exception("Unexpected Microsoft.AspNet.Identity.Owin.SignInStatus enum value: " + result);
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(AccountRegisterViewModel viewModel)
        {
            // Validate if the provided email address is already in use.
            var existingUser = await _userManager.FindByEmailAsync(viewModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", $"The provided email address '{viewModel.Email}' has already been used to register an account. Please sign-in using your existing account.");
            }
            // If the ModelState is valid...
            if (ModelState.IsValid)
            {
                // Instantiate a User object
                var user = new User { UserName = viewModel.Email, Email = viewModel.Email };
                // Create the user
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                // If the user was successfully created...
                if (result.Succeeded)
                {
                    // Sign-in the user and redirect them to the web app's "Home" page 
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // Redirect them to the web app's "Home" page    
                    return RedirectToAction("Index", "Entries");
                }
                // If there were errors...
                foreach (var error in result.Errors)
                {
                    // Add model errors
                    ModelState.AddModelError("", error);
                }
            }
            return View(viewModel);
        }
    }
}
