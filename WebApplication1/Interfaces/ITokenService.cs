using System;
using System.Collections.Generic;
using WebApplication1.Entities;

namespace WebApplication1.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);       
    }
}