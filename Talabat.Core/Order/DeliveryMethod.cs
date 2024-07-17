using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Order
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
        }

        public DeliveryMethod(string shortName, string describtion, string deliveryTime, decimal cost)
        {
            ShortName = shortName;
            Describtion = describtion;
            DeliveryTime = deliveryTime;
            Cost = cost;
        }

        public string ShortName { get; set; }
        public string Describtion { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Cost { get; set; }
    }
}
