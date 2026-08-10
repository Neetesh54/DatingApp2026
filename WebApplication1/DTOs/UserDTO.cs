using System;

namespace WebApplication1.DTOs
{
    public class UserDTO
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string displayName { get; set; }
        public required string Token { get; set; }
        public string? ImageUrl { get; set; }
    }
}