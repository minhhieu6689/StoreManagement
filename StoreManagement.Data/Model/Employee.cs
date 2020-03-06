using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreManagement.Data.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

        public ICollection<Department> Departments { get; set; }
    }
}
