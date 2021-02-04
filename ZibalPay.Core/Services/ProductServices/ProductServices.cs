using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ZibalPay.Core.Extensions;
using ZibalPay.Core.Helpers;
using ZibalPay.Core.Responses;
using ZibalPay.Core.Security;
using ZibalPay.Data.Context;
using ZibalPay.Data.Entities.Products;

namespace ZibalPay.Core.Services.ProductServices
{
    public class ProductServices:IProductServices
    {
        private readonly ZibalPay_Db_Context _context;

        public ProductServices(ZibalPay_Db_Context context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<CreateProductResponse> CreateProduct(Product product, IFormFile image)
        {
            CreateProductResponse response = new CreateProductResponse();
            try
            {

                if (!image.ImageValidator())
                {
                    response.DidError = true;
                    response.Message = "تصویر وارد شده معیوب است";
                    return response;
                }

                //Save Doc Image
                string imgName = CodeGenerators.GenerateGUID("product_");
                string path = "wwwroot/Images/Product";
                string productImage = image.SaveImage(imgName, path, null);

                product.ProductImage = productImage;
                product.CreateDate=DateTime.Now;


                //Add Driver Doc
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();


                

                response.DidError = false;
                response.Message = "عملیات با موفقیت انجام شد";
                response.ReturnValue = product.ProductId;

                return response;
            }
            catch
            {
                response.DidError = true;
                response.Message = "Unhandled Exception Occurred";
                return response;
            }
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
