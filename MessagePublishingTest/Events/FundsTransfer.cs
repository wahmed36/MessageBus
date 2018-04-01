using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MessagePublishingTest.Events
{
    public class FundsTransfer
    {
        [Display(Name = "From Customer Name")]
        public string FromCustomerName { get; set; }
        [Display(Name ="Transfer to Customer Name")]
        public string ToCustomerName { get; set; }
        [Display(Name ="Amount")]
        public double Amount { get; set; }
        public DateTime TranssactionTime { get; set; }

        public FundsTransfer()
        {
            FromCustomerName = ToCustomerName = string.Empty;
            TranssactionTime = DateTime.Now;
        }
    }
}