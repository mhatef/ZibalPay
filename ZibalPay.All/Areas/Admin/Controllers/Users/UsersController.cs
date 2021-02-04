using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.User;
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
    [PermissionChecker((int)Permissions.Users)]
    public class UsersController : Controller
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }


        // GET: Admin/Users
        public async Task<IActionResult> Index(bool id=false)
        {
            ViewData["NotNull"] = id;
            return View(await _userServices.GetAllUsers(id));
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Roles"] = await _userServices.GetAllRoles();
            return View();
        }

        // POST: Admin/Users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserFirstName,UserLastName,UserPassword,UserGender,UserPhoneNumber,UserEmail")] User user, List<int> selectedRole)
        {
            if (ModelState.IsValid)
            {

                if (await _userServices.IsPhoneNumberExist(user.UserPhoneNumber))
                {
                    ModelState.AddModelError("UserPhoneNumber", "شماره تلفن از قبل در سیستم ثبت شده است");
                    ViewData["Roles"] = await _userServices.GetAllRoles();
                    return View();
                }

                if (await _userServices.IsEmailExist(user.UserEmail))
                {
                    ModelState.AddModelError("UserEmail", "ایمیل از قبل در سیستم ثبت شده است");
                    ViewData["Roles"] = await _userServices.GetAllRoles();
                    return View();
                }

                if (await _userServices.CreateUser(user, selectedRole))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("UserFirstName", "خطایی در هنگام افزودن کاربر جدید رخ داد");
            }
            ViewData["Roles"] = await _userServices.GetAllRoles();
            return View();
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roles"] = await _userServices.GetAllRoles();
            ViewData["SelectedRoles"] = await _userServices.GetSelectedRoles(id.Value);
            var editUser = new EditUserVM()
            {
                UserId = user.UserId,
                UserFirstName = user.UserFirstName,
                UserLastName = user.UserLastName,
                UserPhoneNumber = user.UserPhoneNumber,
                UserEmail = user.UserEmail,
                UserGender = user.UserGender
            };
            return View(editUser);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserFirstName,UserLastName,UserGender,UserPhoneNumber,UserEmail")] EditUserVM user, List<int> selectedRole)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (await _userServices.IsPhoneNumberExist(user.UserPhoneNumber,user.UserId))
                {
                    ModelState.AddModelError("UserPhoneNumber", "شماره تلفن از قبل در سیستم ثبت شده است");
                    ViewData["Roles"] = await _userServices.GetAllRoles();
                    ViewData["SelectedRoles"] = await _userServices.GetSelectedRoles(id);
                    return View(user);
                }

                if (await _userServices.IsEmailExist(user.UserEmail,user.UserId))
                {
                    ModelState.AddModelError("UserEmail", "ایمیل از قبل در سیستم ثبت شده است");
                    ViewData["Roles"] = await _userServices.GetAllRoles();
                    ViewData["SelectedRoles"] = await _userServices.GetSelectedRoles(id);
                    return View(user);
                }

                if (await _userServices.UpdateUser(user, selectedRole))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("UserFirstName", "خطایی در هنگام ویرایش کاربر رخ داد");
            }
            ViewData["Roles"] = await _userServices.GetAllRoles();
            ViewData["SelectedRoles"] = await _userServices.GetSelectedRoles(id);
            return View(user);
        }


        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                if (await _userServices.SoftDeleteUser(id))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            throw new Exception("Delete Failed");
        }


        // POST: Admin/Users/Deactivate/5
        [HttpPost, ActionName("Deactivate")]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                if (await _userServices.DeactivateUser(id))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            throw new Exception("Deactivate Failed");
        }

        public async Task<IActionResult> ChangePassword(int id)
        {
            ViewData["UserFullName"] = await _userServices.GetUserFullName(id);
            return View(new ChangeUserPasswordVm(){UserId = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordVm change)
        {
            if (ModelState.IsValid)
            {
                if (await _userServices.ChangeUserPassword(change))
                {
                    return RedirectToAction("Edit", new {id = change.UserId});
                }
            }
            return View();
        }
    }
}
