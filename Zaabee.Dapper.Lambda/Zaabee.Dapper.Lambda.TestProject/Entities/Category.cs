using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Categories")]
    public class Category
    {
        [SqlLamColumn(Name = "Category ID")]
        public int CategoryId { get; set; }

        public int GetCategoryId()
        {
            return CategoryId;
        }
        
        [SqlLamColumn(Name = "Category Name")]
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; }
    }
}
