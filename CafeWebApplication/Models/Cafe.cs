using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class Cafe
    {
        public Cafe()
        {
            Employees = new HashSet<Employee>();
            MenuItems = new HashSet<MenuItem>();
            Tables = new HashSet<Table>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва кав'ярні")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Адреса")]
        public string Address { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<Table> Tables { get; set; }
    }
}
