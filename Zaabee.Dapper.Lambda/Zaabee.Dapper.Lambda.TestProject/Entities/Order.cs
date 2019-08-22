using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Orders")]
    public class Order
    {
        [SqlLamColumn(Name = "Order ID")]
        public int OrderId { get; set; }

        [SqlLamColumn(Name = "Ship Name")]
        public string ShipName { get; set; }

        [SqlLamColumn(Name = "Ship Region")]
        public string ShipRegion { get; set; }

        [SqlLamColumn(Name = "Required Date")]
        public DateTime RequiredDate { get; set; }

        [SqlLamColumn(Name = "Shipped Date")]
        public DateTime ShippedDate { get; set; }

        public List<Product> Products { get; set; }
    }
}
