using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Column( "Order ID")]
        public int OrderId { get; set; }

        [Column( "Ship Name")]
        public string ShipName { get; set; }

        [Column( "Ship Region")]
        public string ShipRegion { get; set; }

        [Column( "Required Date")]
        public DateTime RequiredDate { get; set; }

        [Column( "Shipped Date")]
        public DateTime ShippedDate { get; set; }

        public List<Product> Products { get; set; }
    }
}
