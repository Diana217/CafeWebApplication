using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        [Display(Name = "Кав'ярня")]
        public int CafeId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Статус")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Номер")]
        public int Number { get; set; }

        [Display(Name = "Кав'ярня")]
        public virtual Cafe Cafe { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
