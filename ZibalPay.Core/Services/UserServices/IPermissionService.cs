using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZibalPay.Data.Entities.Users;

namespace ZibalPay.Core.Services.UserServices
{
    public interface IPermissionService:IDisposable
    {
        Task<List<Permission>> GetAllPermission();
        Task<List<int>> PermissionsRole(int roleId);
        Task<bool> UpdatePermissionsRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string userPhoneNumber);
    }
}
