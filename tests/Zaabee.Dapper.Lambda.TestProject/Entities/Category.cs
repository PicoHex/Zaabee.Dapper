using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Column("Category ID")]
        public int CategoryId { get; set; }

        public int GetCategoryId()
        {
            return CategoryId;
        }
        
        [Column("Category Name")]
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; }
    }
}
