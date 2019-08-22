using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Order Details")]
    public class OrderDetails
    {
        [SqlLamColumn(Name = "Order ID")]
        public int OrderId { get; set; }

        [SqlLamColumn(Name = "Product ID")]
        public int ProductId { get; set; }
    }
}
