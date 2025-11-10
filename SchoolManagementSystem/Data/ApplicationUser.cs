using Microsoft.AspNetCore.Identity;
using System; // Required for DateTime, etc.

namespace SchoolManagementSystem.Data // or SchoolManagementSystem.Data, depending on your project structure
{
    // Inherit from IdentityUser to get all standard Identity properties
    public class ApplicationUser : IdentityUser
    {
        // Add your custom properties here (optional, but a common reason for custom class)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;
    }
}