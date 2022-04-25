using System.ComponentModel.DataAnnotations;

namespace CafeWebApplication
{
    public partial class ItemType
    {
        public ItemType()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Тип позиції меню")]
        public string Type { get; set; } = null!;

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
