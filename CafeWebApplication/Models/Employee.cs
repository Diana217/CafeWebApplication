using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class Employee
    {
        public Employee()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        [Display(Name = "Кав'ярня")]
        public int CafeId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Тип працівника")]
        public bool EmployeeType { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "ПІБ працівника")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата прийому на роботу")]
        public DateTime DateOfEmployment { get; set; }
        [Display(Name = "Дата звільнення з роботи")]
        public DateTime? DateOfRelease { get; set; }
        [Display(Name = "Кав'ярня")]
        public virtual Cafe Cafe { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
