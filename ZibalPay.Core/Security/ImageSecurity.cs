using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ZibalPay.Core.Security
{
    public static class ImageSecurity
    {
        public static bool ImageValidator(this IFormFile file)
        {
            if (file.Length > 0 && file != null)
            {
                try
                {
                    System.Drawing.Image.FromStream(file.OpenReadStream());
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
