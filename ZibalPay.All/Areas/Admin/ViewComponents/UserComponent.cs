using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZibalPay.All.Areas.Admin.ViewComponents
{
    public class UserComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user  = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return await Task.FromResult((IViewComponentResult)View("UserCard",user));
        }
    }
}
