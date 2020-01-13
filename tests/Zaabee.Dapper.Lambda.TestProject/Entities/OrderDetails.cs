using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Order Details")]
    public class OrderDetails
    {
        [Column( "Order ID")]
        public int OrderId { get; set; }

        [Column( "Product ID")]
        public int ProductId { get; set; }
    }
}
