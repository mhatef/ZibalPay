using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.User;
using ZibalPay.Core.Security;
using ZibalPay.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ZibalPay.Core.Services.UserServices
{
    public class AuthServices:IAuthServices
    {
        private readonly ZibalPay_Db_Context _context;

        public AuthServices(ZibalPay_Db_Context context)
        {
            _context = context;
        }


        public async Task<LoginUserVm> UserExist(string userPhone, string userPassword)
        {
            var hashPassword = PasswordHelper.EncodePasswordMd5(userPassword);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.IsActive &&
                                                                      u.UserPhoneNumber == userPhone &&
                                                                      u.UserPassword == hashPassword);
            if (user == null)
            {
                return null;
            }

            return new LoginUserVm()
            {
                Fullname=user.UserFirstName+" "+user.UserLastName,
                UserId = user.UserId,
                Username = user.UserPhoneNumber
            };
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
