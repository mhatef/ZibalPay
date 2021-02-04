using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZibalPay.Core.Services.ProductServices;
using ZibalPay.Core.Services.UserServices;

namespace ZibalPay.Web.ViewComponents
{
    public class GetProductComponent:ViewComponent
    {
        private readonly IProductServices _productServices;

        public GetProductComponent(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productServices.GetAllProducts();
            return await Task.FromResult((IViewComponentResult)View("GetProducts",products));
        }
    }


    public class GetCartComponent : ViewComponent
    {
        private readonly ICustomerServices _customerServices;

        public GetCartComponent(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _customerServices.GetUserCart(User.Identity!.Name);
            return await Task.FromResult((IViewComponentResult) View("GetUserCart", cart));
        }
    }
}
