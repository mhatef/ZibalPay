using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ZibalPay.Core.Security;

namespace ZibalPay.All.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(0)] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult UserInfoCard()
        {
            var name = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            ViewData["name"] = "";
            return View();
        }
    }
}
