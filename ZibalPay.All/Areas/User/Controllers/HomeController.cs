using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ZibalPay.Core.DTOs.Customer;
using ZibalPay.Core.Services.UserServices;

namespace ZibalPay.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly ICustomerServices _customerServices;

        public HomeController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _customerServices.GetCustomerInfo(User.Identity!.Name));
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            if (await _customerServices.AddToCart(id,User.Identity!.Name))
            {
                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        public IActionResult Payment(int id)
        {
            var orderRequest = _customerServices.GetOrderForPayment(id);
            if (orderRequest==null)
            {
                return NotFound("Order Not Found");
            }

            var paymentResponse = _customerServices.RequestForPayment(orderRequest);

            if (paymentResponse.result == "100")
            {
                return Redirect($"https://gateway.zibal.ir/start/{paymentResponse.trackId}");
            }
            
            return View(paymentResponse);
        }

        public IActionResult PaymentCallBack(int success,string trackId,int orderId,int status)
        {
            if (success ==1 || status==2)
            {
                var response = _customerServices.VerifyRequest(new verifyRequest()
                {
                    merchant = "zibal",
                    trackId = trackId
                });
                if (response.result == "100")
                {
                    _customerServices.AcceptOrder(orderId);
                    ViewData["success"] = true;
                    return View(response);
                }
            }
            ViewData["success"] = null;
            return View();
        }

    }
}
