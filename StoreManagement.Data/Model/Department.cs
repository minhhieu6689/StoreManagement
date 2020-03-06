using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreManagement.Data.Model
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } 

    }
}
