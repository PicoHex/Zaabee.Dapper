using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Extensions.TestProject
{
    [Table("test_po")]
    public class TestPO
    {
        [Key] [Column("id")] public Guid TestId { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("create_time")] public DateTime CreateTime { get; set; }
    }
}