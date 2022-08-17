using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkyPlanner.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        [Min(1, ErrorMessage = "The Account is required")]
        public int AccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<OrderLineItem> OrderLineItem { get; set; }

    }
}
