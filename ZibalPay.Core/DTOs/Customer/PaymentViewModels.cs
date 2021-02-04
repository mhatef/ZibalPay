using System;
using System.Collections.Generic;
using System.Text;

namespace ZibalPay.Core.DTOs.Customer
{
    
        public class makeRequest
        {
            public string merchant { get; set; }
            public string orderId { get; set; }
            public int amount { get; set; }
            public string callbackUrl { get; set; }
            public string description { get; set; }
            public string mobile { get; set; }
        }

        public class makeRequest_response
        {
            public string trackId { get; set; }
            public string result { get; set; }
            public string message { get; set; }
        }

        public class verifyRequest
        {
            public string merchant { get; set; }
            public string trackId { get; set; }
        }

        public class verifyResponse
        {
            public string paidAt { get; set; }
            public string status { get; set; }
            public string amount { get; set; }
            public string result { get; set; }
            public string message { get; set; }
        }
    }

