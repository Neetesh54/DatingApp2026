using System;
using System.Collections.Generic;
using WebApplication1.DTOs;
using WebApplication1.Entities;
using WebApplication1.Interfaces;

namespace WebApplication1.Extensions
{
    public static class AppUserExtensions
    {
        public static UserDTO AsUserDTO(this AppUser user, ITokenService tokenService)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                displayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            };
        }
    }
}