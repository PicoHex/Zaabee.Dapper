using System;

namespace Zaabee.Dapper.Lambda.TestProject.Entities
{
    [SqlLamTable(Name = "Employees")]
    public class Employee
    {
        [SqlLamColumn(Name = "Employee ID")]
        public int EmployeeId { get; set; }

        [SqlLamColumn(Name = "First Name")]
        public string FirstName { get; set; }

        [SqlLamColumn(Name = "Last Name")]
        public string LastName { get; set; }

        [SqlLamColumn(Name = "City")]
        public string City { get; set; }

        [SqlLamColumn(Name = "Title")]
        public string Title { get; set; }

        [SqlLamColumn(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
