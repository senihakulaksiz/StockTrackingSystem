using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Entities
{
    public class ItemProperty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; } 

        [Required]
        [MaxLength(50)]
        public string BranchName { get; set; } = null!; // Şube adı

        [Required]
        public bool IsActive { get; set; } // Bu şubede aktif kullanılıyor mu?

    }
}
