using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class MenuOrder
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Кількість")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва пощиції меню")]
        public virtual MenuItem MenuItem { get; set; } = null!;
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Замовлення")]
        public virtual Order Order { get; set; } = null!;
    }
}
