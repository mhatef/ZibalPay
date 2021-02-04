using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZibalPay.Core.Responses;
using ZibalPay.Data.Entities.Products;

namespace ZibalPay.Core.Services.ProductServices
{
    public interface IProductServices:IDisposable
    {
        Task<List<Product>> GetAllProducts();
        Task<CreateProductResponse> CreateProduct(Product product,IFormFile image);
    }
}
