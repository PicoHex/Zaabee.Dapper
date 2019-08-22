using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Products")]
    public class Product
    {
        [Column( "Product ID")]
        public int ProductId { get; set; }

        public int GetProductId()
        {
            return ProductId;
        }
        
        [Column( "Product Name")]
        public string ProductName { get; set; }

        [Column( "English Name")]
        public string EnglishName { get; set; }
        
        [Column( "Category ID")]
        public int CategoryId { get; set; }

        [Column( "Unit Price")]
        public double UnitPrice { get; set; }

        [Column( "Reorder Level")]
        public int ReorderLevel { get; set; }

        [Column( "Reorder Level")]
        public int? NullableReorderLevel { get { return ReorderLevel; } }

        [Column( "Discontinued")]
        public bool Discontinued { get; set; }

        public Category Category { get; set; }
    }
}
