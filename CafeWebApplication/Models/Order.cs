using System;
using System.Collections.Generic;
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
        public int WaiterId { get; set; }
        public int TableId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Столик")]

        public virtual Table Table { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Офіціант")]
        public virtual Employee Waiter { get; set; } = null!;
        public virtual ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
