using SchoolManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign key to ApplicationUser who created the entity
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser? CreatedBy { get; set; }

        // Foreign key to ApplicationUser who last updated the entity
        public string? UpdatedById { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual ApplicationUser? UpdatedBy { get; set; }
    }
}
