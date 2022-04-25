using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class Order
    {
        public Order()
        {
            MenuOrders = new HashSet<MenuOrder>();
        }

        public int Id { get; set; }

        [Display(Name = "Офіціант")]
        public int WaiterId { get; set; }

        [Display(Name = "Столик")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Столик")]
        public virtual Table Table { get; set; } = null!;

        [Display(Name = "Офіціант")]
        public virtual Employee Waiter { get; set; } = null!;
        public virtual ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
