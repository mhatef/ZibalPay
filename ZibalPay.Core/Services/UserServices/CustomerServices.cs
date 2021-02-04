using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZibalPay.Core.DTOs.Customer;
using ZibalPay.Data.Context;
using ZibalPay.Data.Entities.Products;

namespace ZibalPay.Core.Services.UserServices
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ZibalPay_Db_Context _context;

        public CustomerServices(ZibalPay_Db_Context context)
        {
            _context = context;
        }


        public async Task<CustomerInfo> GetCustomerInfo(string phoneNumber)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.UserPhoneNumber == phoneNumber);

            if (user == null)
            {
                return null;
            }

            return new CustomerInfo()
            {
                CustomerId = user.UserId,
                CustomerName = user.UserFirstName + " " + user.UserLastName
            };
        }

        public async Task<bool> AddToCart(int productId, string userPhoneNumber)
        {
            try
            {
                var customer = await _context.Users
                    .SingleOrDefaultAsync(u => u.UserPhoneNumber == userPhoneNumber);

                var product = await _context.Products.FindAsync(productId);

                if (customer == null || product == null)
                {
                    return false;
                }

                var order = await _context.Orders
                    .SingleOrDefaultAsync(o => o.UserId == customer.UserId && !o.IsFinally);

                if (order == null)
                {
                    // bayad factor jadid ijad shavad
                    await _context.Orders.AddAsync(new Order()
                    {
                        UserId = customer.UserId,
                        OrderSum = product.ProductPrice,
                        CreateDate = DateTime.Now,
                        IsFinally = false,
                        OrderDetails = new List<OrderDetail>()
                        {
                            new OrderDetail()
                            {
                                ProductId = productId,
                                Count = 1,
                                Price = product.ProductPrice
                            }
                        }
                    });
                }
                else
                {
                    // factor baz darad
                    await _context.OrderDetails.AddAsync(new OrderDetail()
                    {
                        OrderId =order.OrderId,
                        ProductId = productId,
                        Price = product.ProductPrice,
                        Count = 1
                    });

                    order.OrderSum += product.ProductPrice;
                    _context.Orders.Update(order);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Order> GetUserCart(string userPhoneNumber)
        {
            var customer = await _context.Users
                .SingleOrDefaultAsync(u => u.UserPhoneNumber == userPhoneNumber);

            if (customer == null)
            {
                return null;
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .SingleOrDefaultAsync(o => o.UserId == customer.UserId && !o.IsFinally);

            return order;


        }

        public makeRequest GetOrderForPayment(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order==null)
            {
                return null;
            }

            var request = new makeRequest()
            {
                orderId = orderId.ToString(),
                amount = order.OrderSum,
                callbackUrl = "http://localhost:50548/User/Home/PaymentCallBack",
                description = "پرداخت آزمایشی",
                merchant = "zibal"

            };
            return request;
        }


        public makeRequest_response RequestForPayment(makeRequest request)
        {
            try
            {
                string url = "https://gateway.zibal.ir/v1/request"; // url
                
                var httpResponse = HttpRequestToZibal(url, JsonConvert.SerializeObject(request));  // get Response
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) // make stream reader
                {
                    var responseText = streamReader.ReadToEnd(); // read Response
                    makeRequest_response response = JsonConvert.DeserializeObject<makeRequest_response>(responseText); // Deserilize as response class object
                    // you can access track id with item.trackId , result with item.result and message with item.message
                    // in asp.net you can use Response.Redirect("https://gateway.zibal.ir/start/item.trackId"); for start gateway and redirect to third-party gateway page
                    // also you can use Response.Redirect("https://gateway.zibal.ir/start/item.trackId/direct"); for start gateway page directly

                    return response;
                }
            }
            catch
            {
                return null;
            }
        }

        public verifyResponse VerifyRequest(verifyRequest request)
        {
            try
            {
                string url = "https://gateway.zibal.ir/v1/verify"; // url
                 
                var httpResponse =HttpRequestToZibal(url, JsonConvert.SerializeObject(request));  // get Response
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) // make stream reader
                {
                    var responseText = streamReader.ReadToEnd(); // read Response
                    verifyResponse response = JsonConvert.DeserializeObject<verifyResponse>(responseText); // Deserilize as response class object
                    // you can access paidAt time with item.paidAt , result with item.result , message with item.message , status with item.status and amount with item.amount
                    return response;
                }
            }
            catch
            {
                return null;
            }
        }

        public void AcceptOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            order.IsFinally = true;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        private static HttpWebResponse HttpRequestToZibal(string url, string data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url); // make request
            httpWebRequest.ContentType = "application/json; charset=utf-8"; // content of request -> must be JSON
            httpWebRequest.Method = "POST"; // method of request -> must be POST
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data); // send request
                streamWriter.Flush(); // flush stream
            }
            return (HttpWebResponse)httpWebRequest.GetResponse(); // get Response
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
