using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZibalPay.Core.Services.ProductServices;

namespace ZibalPay.Web.Areas.Admin.Controllers.Product
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productServices.GetAllProducts());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Data.Entities.Products.Product product,IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _productServices.CreateProduct(product, productImage);
                if (!res.DidError)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("ProductTitle", res.Message);
            }

            return View(product);
        }
    }
}
