using System;
using System.Collections.Generic;
using System.Text;

namespace ZibalPay.Core.Helpers
{
    public class CodeGenerators
    {
        public static int PhoneActiveCodeGenerator()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999);
        }

        public static string GenerateGUID(string name)
        {
            return name + Guid.NewGuid().ToString().Replace("-", "");
        }

        public static int TokenGenerator()
        {
            Random rnd = new Random();
            return rnd.Next(100000000, 999999999);
        }
    }
}
