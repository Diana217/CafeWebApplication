using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class MenuOrder
    {
        public int Id { get; set; }
        [Display(Name = "Позиція меню")]
        public int MenuItemId { get; set; }
        [Display(Name = "Замовлення")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Кількість")]
        public int Amount { get; set; }

        [Display(Name = "Позиція меню")]
        public virtual MenuItem MenuItem { get; set; } = null!;
        [Display(Name = "Замовлення")]
        public virtual Order Order { get; set; } = null!;
    }
}
