using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.User;

namespace ZibalPay.Core.Services.UserServices
{
    public interface IAuthServices:IDisposable
    {
        Task<LoginUserVm> UserExist(string userPhone, string userPassword);
    }
}
