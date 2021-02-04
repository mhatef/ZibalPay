using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.User;
using ZibalPay.Core.Security;
using ZibalPay.Data.Context;
using ZibalPay.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace ZibalPay.Core.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly ZibalPay_Db_Context _context;

        public UserServices(ZibalPay_Db_Context context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<bool> CreateRole(Role role)
        {
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRole(Role role)
        {
            try
            {
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.UserSelectedRoles)
                .ThenInclude(u => u.Role).ToListAsync();
        }

        public async Task<List<User>> GetAllUsers(bool justDeletedUsers)
        {
            if (justDeletedUsers)
            {
                return await _context.Users
                    .Where(u => u.IsDelete)
                    .Include(u => u.UserSelectedRoles)
                    .ThenInclude(u => u.Role)
                    .IgnoreQueryFilters().ToListAsync();
            }

            return await _context.Users
                .Include(u => u.UserSelectedRoles)
                .ThenInclude(u => u.Role).ToListAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> CreateUser(User user, List<int> selectedRoles)
        {
            try
            {
                user.IsActive = true;
                user.IsDelete = false;
                user.UserPassword = PasswordHelper.EncodePasswordMd5(user.UserPassword);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();


                // Add Selected Roles
                foreach (int selectedRole in selectedRoles)
                {
                    await _context.UserSelectedRoles.AddAsync(new UserSelectedRole()
                    {
                        UserId = user.UserId,
                        RoleId = selectedRole
                    });
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(EditUserVM editUser, List<int> selectedRoles)
        {
            try
            {
                var user = await _context.Users.FindAsync(editUser.UserId);
                
                user.UserFirstName = editUser.UserFirstName;
                user.UserLastName = editUser.UserLastName;
                user.UserGender = editUser.UserGender;
                user.UserPhoneNumber = editUser.UserPhoneNumber;
                user.UserEmail = editUser.UserEmail;

                _updateUser(user);

                // Remove Previous Roles
                await _context.UserSelectedRoles
                    .Where(usr=>usr.UserId == user.UserId)
                    .ForEachAsync(usr =>
                    _context.UserSelectedRoles.Remove(usr));


                // Add Selected Roles
                foreach (int selectedRole in selectedRoles)
                {
                    await _context.UserSelectedRoles.AddAsync(new UserSelectedRole()
                    {
                        UserId = user.UserId,
                        RoleId = selectedRole
                    });
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteUser(int userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.UserId == userId).IgnoreQueryFilters().SingleAsync();
                user.IsDelete = !user.IsDelete;
                _updateUser(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateUser(int userId)
        {
            try
            {
                var user = await GetUserById(userId);
                user.IsActive = !user.IsActive;
                _updateUser(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsPhoneNumberExist(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.UserPhoneNumber == phoneNumber);
        }

        public async Task<bool> IsPhoneNumberExist(string phoneNumber, int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserPhoneNumber == phoneNumber && u.UserId != userId);
        }

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Users.AnyAsync(u => u.UserEmail == email);
        }

        public async Task<bool> IsEmailExist(string email, int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserEmail == email && u.UserId != userId);
        }

        public async Task<List<int>> GetSelectedRoles(int userId)
        {
            return await _context.UserSelectedRoles
                .Where(u => u.UserId == userId)
                .Select(usr => usr.RoleId).ToListAsync();
        }

        public async Task<string> GetUserFullName(int userId)
        {
            return await _context.Users.Where(u => u.UserId == userId)
                .Select(u => u.UserFirstName +" "+ u.UserLastName).SingleAsync();
        }

        public async Task<bool> ChangeUserPassword(ChangeUserPasswordVm changeUserPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(changeUserPassword.UserId);
                if (changeUserPassword.Password != changeUserPassword.RePassword)
                {
                    return false;
                }

                var hashPassword = PasswordHelper.EncodePasswordMd5(changeUserPassword.Password);
                user.UserPassword = hashPassword;
                _updateUser(user);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }

        private void _updateUser(User user)
        {
            _context.Users.Update(user);
        }
    }
}
