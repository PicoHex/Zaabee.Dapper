namespace Zaabee.Dapper.Extensions.TestProject.POCOs;

[Table("order")]
public class Order
{
    [Key] [Column("id")] public Guid Id { get; set; }
    [Column("buyer_name")] public string BuyerName { get; set; }
    [Column("address")]public Address Address { get; set; }
    private IList<OrderDetail> _details = new List<OrderDetail>();

    [ForeignKey("order_id")]
    public IList<OrderDetail> Details
    {
        get => _details;
        set => _details = value ?? new List<OrderDetail>();
    }

    [NotMapped] public decimal TotalPrice => Details.Sum(d => d.UnitPrice * d.Quantity);
}

[ComplexType]
public class Address
{
    [Column("country")] public string Country { get; set; }
    [Column("state")] public string State { get; set; }
    [Column("city")] public string City { get; set; }
    [Column("street")] public string Street { get; set; }
}

[Table("order_detail")]
public class OrderDetail
{
    [Column("id")] public Guid Id { get; set; }
    [Column("order_id")] public Guid OrderId { get; set; }
    [Column("sku")] public string Sku { get; set; }
    [Column("unit_price")] public decimal UnitPrice { get; set; }
    [Column("quantity")] public int Quantity { get; set; }
}