using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class MenuItem
    {
        public MenuItem()
        {
            MenuOrders = new HashSet<MenuOrder>();
        }

        public int Id { get; set; }
        [Display(Name = "Кав'ярня")]
        public int CafeId { get; set; }
        [Display(Name = "Тип позиції меню")]
        public int ItemTypeId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва позиції меню")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Статус")]
        public bool Status { get; set; }

        [Display(Name = "Кав'ярня")]
        public virtual Cafe Cafe { get; set; } = null!;
        [Display(Name = "Тип позиції меню")]
        public virtual ItemType ItemType { get; set; } = null!;
        public virtual ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
