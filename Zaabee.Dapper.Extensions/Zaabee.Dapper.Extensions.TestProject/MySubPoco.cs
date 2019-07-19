using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Extensions.TestProject
{
    [Table("my_sub_poco")]
    public class MySubPoco
    {
        [Key] [Column("id")] public Guid Id { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("remark")] public string Remark { get; set; }
    }
}