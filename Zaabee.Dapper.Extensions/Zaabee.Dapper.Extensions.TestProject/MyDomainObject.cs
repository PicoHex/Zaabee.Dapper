using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Extensions.TestProject
{
    [Table("my_poco")]
    public class MyDomainObject
    {
        [Key] [Column("id")] public Guid Id { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("gender")] public Gender Gender { get; set; }

        private DateTime _birthday;

        [Column("birthday")]
        public DateTime Birthday
        {
            get => _birthday;
            set => _birthday = value;
        }

        private DateTime _createTime;

        [Column("create_time")]
        public DateTime CreateTime
        {
            get => _createTime;
            set => _createTime = value;
        }
    }

    public enum Gender : byte
    {
        Male,
        Female
    }
}