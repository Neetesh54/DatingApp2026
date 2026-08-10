using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Entities;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApplication1.Interfaces;
using WebApplication1.Extensions;


namespace WebApplication1.Controllers
{
    public class AccountController(AppDbContext appContext, ITokenService tokenservice) : BaseApiController
    {
        // Controller methods and actions go here

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
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

            return Ok(user.AsUserDTO(tokenservice));
        }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        // Implementation for login logic
        var user = await appContext.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());

        if (user == null) return Unauthorized("Invalid email or password");

        var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid email or password");
        }

        return Ok(user.AsUserDTO(tokenservice));   
    }

    private async Task<bool> EmailExists(string email)
    {
        return await appContext.Users
                .AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
}