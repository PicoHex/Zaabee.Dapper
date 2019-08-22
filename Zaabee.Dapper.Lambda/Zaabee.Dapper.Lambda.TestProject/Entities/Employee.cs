using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Column("Employee ID")]
        public int EmployeeId { get; set; }

        [Column("First Name")]
        public string FirstName { get; set; }

        [Column("Last Name")]
        public string LastName { get; set; }

        [Column("City")]
        public string City { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
