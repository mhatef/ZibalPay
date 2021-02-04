using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.User;
using ZibalPay.Data.Entities.Users;

namespace ZibalPay.Core.Services.UserServices
{
    public interface IUserServices:IDisposable
    {
        #region Role

        Task<List<Role>> GetAllRoles();
        Task<Role> GetRoleById(int roleId);
        Task<bool> CreateRole(Role role);
        Task<bool> UpdateRole(Role role);

        #endregion

        #region User

        Task<List<User>> GetAllUsers();
        Task<List<User>> GetAllUsers(bool justDeletedUsers);
        Task<User> GetUserById(int userId);
        Task<bool> CreateUser(User user,List<int> selectedRoles);
        Task<bool> UpdateUser(EditUserVM user,List<int> selectedRoles);
        Task<bool> SoftDeleteUser(int userId);
        Task<bool> DeactivateUser(int userId);
        Task<bool> IsPhoneNumberExist(string phoneNumber);
        Task<bool> IsPhoneNumberExist(string phoneNumber,int userId);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsEmailExist(string email,int userId);
        Task<List<int>> GetSelectedRoles(int userId);
        Task<string> GetUserFullName(int userId);
        Task<bool> ChangeUserPassword(ChangeUserPasswordVm changeUserPassword);

        #endregion
    }
}
