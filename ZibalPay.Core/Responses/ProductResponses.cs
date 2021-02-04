using System;
using System.Collections.Generic;
using System.Text;

namespace ZibalPay.Core.Responses
{
    public class CreateProductResponse
    {
        public object ReturnValue { get; set; }
        public bool DidError { get; set; }
        public string Message { get; set; }
    }
}
