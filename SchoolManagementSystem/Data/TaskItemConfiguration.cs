using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            // Configure BaseEntity properties for auditing
            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Configure relationships for CreatedBy and UpdatedBy (eager/lazy compatible)
            builder.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.UpdatedBy)
                .WithMany()
                .HasForeignKey(t => t.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TaskItem-specific properties
            builder.Property(t => t.Title)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete if user is deleted
        }
    }
}