using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Entities;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    public class AccountController(AppDbContext appContext) : BaseApiController
    {
        // Controller methods and actions go here

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDto)
        {
            // Implementation for registration logic
            if(await EmailExists(registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }
            
            var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            appContext.Users.Add(user);
            await appContext.SaveChangesAsync();

            return Ok(user);
        }
    
    private async Task<bool> EmailExists(string email)
    {
        return await appContext.Users
                .AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
}