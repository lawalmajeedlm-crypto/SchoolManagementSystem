using SchoolManagementSystem.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class TaskItem : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }

}
