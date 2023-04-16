﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Project.MVCUI.OutherRequestModel
{
    public class PaymentRequestModel
    {
        public string CardUserName { get; set; }
        public string SecurityNumber { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set;}
        public decimal ShoppingPrice { get; set; }

    }
}