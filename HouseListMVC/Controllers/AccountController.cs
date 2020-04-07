using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseListMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseListMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> UserMgr;
        private readonly SignInManager<AppUser> SignInMgr;
        private readonly RoleManager<AppRole> RoleMgr;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 RoleManager<AppRole> roleManager)
            {
                this.UserMgr = userManager;
                this.SignInMgr = signInManager;
                this.RoleMgr = roleManager;
            }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register()
        {            
            return View();
        }

        [AllowAnonymous]
        [HttpGet][HttpPost]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await UserMgr.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }
        

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(AppUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName,  Email = model.Email, Seller = model.Seller};
                var result = await UserMgr.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    result = await UserMgr.AddToRoleAsync(user, model.Seller ? "Seller" : "Buyer");
                    await SignInMgr.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }


            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AppLogin model, string returnUrl)
        {
            if (ModelState.IsValid)
            {                
                var result = await SignInMgr.PasswordSignInAsync(
                   model.Email, model.Password, isPersistent:model.RememberMe, false);

                

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }                    
                }

                
                ModelState.AddModelError("", "Invalid email or password");                

            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await SignInMgr.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //#region API Calls

        //public bool GetRole()
        //{
        //    return User.IsInRole("Seller"); 
        //}

        //#endregion

    }
}