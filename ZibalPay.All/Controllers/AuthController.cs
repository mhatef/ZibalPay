using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.User;
using ZibalPay.Core.Services.UserServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ZibalPay.All.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login(LoginVm login, string ReturnUrl = "/admin")
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            LoginUserVm user = await _authServices.UserExist(login.UserPhoneNumber, login.Password);
            if (user != null)
            {
                var claims = new List<Claim>()
                    {
                    new Claim(ClaimTypes.NameIdentifier,user.Fullname),
                        new Claim(ClaimTypes.Name,user.Username)
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe
                };
                await HttpContext.SignInAsync(principal, properties);

                ViewBag.IsSuccess = true;



                return Redirect(ReturnUrl);

            }

            ModelState.AddModelError("UserPhoneNumber", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);
        }


        [Route("/UserLogin")]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [Route("/UserLogin")]
        public async Task<IActionResult> UserLogin(LoginVm login, string ReturnUrl = "/User")
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            LoginUserVm user = await _authServices.UserExist(login.UserPhoneNumber, login.Password);
            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Fullname),
                    new Claim(ClaimTypes.Name,user.Username)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = login.RememberMe
                };
                await HttpContext.SignInAsync(principal, properties);

                return Redirect(ReturnUrl);

            }

            ModelState.AddModelError("UserPhoneNumber", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);
        }

        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
