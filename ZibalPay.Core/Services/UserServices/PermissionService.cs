using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZibalPay.Data.Context;
using ZibalPay.Data.Entities.Users;

namespace ZibalPay.Core.Services.UserServices
{
    public class PermissionService:IPermissionService
    {
        private readonly ZibalPay_Db_Context _context;

        public PermissionService(ZibalPay_Db_Context context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetAllPermission()
        {
            return await _context.Permissions.ToListAsync();
        }

        private void AddPermissionsToRole(int roleId, List<int> permission)
        {
            foreach (var p in permission)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    PermissionId = p,
                    RoleId = roleId
                });
            }

            _context.SaveChanges();
        }

        public async Task<List<int>> PermissionsRole(int roleId)
        {
            return await _context.RolePermissions
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId).ToListAsync();
        }

        public async Task<bool> UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            try
            {
                if (await _context.RolePermissions.AnyAsync(p=>p.RoleId==roleId))
                {
                    _context.RolePermissions.Where(p => p.RoleId == roleId)
                        .ToList().ForEach(p => _context.RolePermissions.Remove(p));
                }

                AddPermissionsToRole(roleId, permissions);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool CheckPermission(int permissionId, string userPhoneNumber)
        {
            int userId = _context.Users.Single(u => u.UserPhoneNumber == userPhoneNumber).UserId;

            List<int> UserRoles = _context.UserSelectedRoles
                .Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _context.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
