using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZibalPay.Core.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZibalPay.Data.Context;
using ZibalPay.Data.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using ZibalPay.Core.Enums;
using ZibalPay.Core.Security;

namespace ZibalPay.All.Areas.Admin.Controllers.Users
{
    [Area("Admin")]
    //[Authorize]
    [PermissionChecker((int)Permissions.Roles)]
    public class RolesController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IPermissionService _permissionService;

        public RolesController(IUserServices userServices, IPermissionService permissionService)
        {
            _userServices = userServices;
            _permissionService = permissionService;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            return View(await _userServices.GetAllRoles());
        }

        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleTitle")] Role role)
        {
            if (ModelState.IsValid)
            {
                if (await _userServices.CreateRole(role))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("RoleTitle", "Create Error");
            }
            return View(role);
        }

        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _userServices.GetRoleById(id.Value);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleTitle")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (await _userServices.UpdateRole(role))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("RoleTitle", "Create Error");
            }
            return View(role);
        }


        public async Task<IActionResult> GetPermissions(int id)
        {
            ViewData["selected"] = await _permissionService.PermissionsRole(id);
            return PartialView(await _permissionService.GetAllPermission());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPermissions(int id,List<int> selectedPermissions)
        {
            if (ModelState.IsValid)
            {
                if (await _permissionService.UpdatePermissionsRole(id,selectedPermissions))
                {
                    return RedirectToAction("Index");
                }
            }

            return BadRequest("Error");
        }
    }
}
