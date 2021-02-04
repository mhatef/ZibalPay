using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZibalPay.Core.DTOs.Customer;
using ZibalPay.Data.Entities.Products;

namespace ZibalPay.Core.Services.UserServices
{
    public interface ICustomerServices:IDisposable
    {
        Task<CustomerInfo> GetCustomerInfo(string phoneNumber);
        Task<bool> AddToCart(int productId, string userPhoneNumber);
        Task<Order> GetUserCart(string userPhoneNumber);


        #region Payment

        makeRequest GetOrderForPayment(int orderId);
        makeRequest_response RequestForPayment(makeRequest request);
        verifyResponse VerifyRequest(verifyRequest request);
        void AcceptOrder(int orderId);

        #endregion
    }
}
