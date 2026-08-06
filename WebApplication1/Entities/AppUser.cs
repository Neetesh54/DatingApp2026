using System;
namespace WebApplication1.Entities
{
    public class AppUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}