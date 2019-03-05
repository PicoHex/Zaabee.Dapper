using System;

namespace Zaabee.Dapper.UnitOfWork.TestProject
{
    public class MyDomainObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public enum Gender : byte
    {
        Male,
        Female
    }
}