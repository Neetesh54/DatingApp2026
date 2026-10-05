using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using WebApplication1.DTOs;

namespace WebApplication1.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var memberData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
        var mebmers = System.Text.Json.JsonSerializer.Deserialize<List<SeedUserDTO>>(memberData);

        if (mebmers == null)
        {
            Console.WriteLine("No members found in the seed data.");
            return;
        }

        foreach (var member in mebmers)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email.ToLower(),
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DateOfBirth = member.DateOfBirth,
                    ImageUrl = member.ImageUrl,
                    DisplayName = member.DisplayName,
                    Created = member.Created,
                    LastActive = member.LastActive,
                    Gender = member.Gender,
                    Description = member.Description,
                    City = member.City,
                    Country = member.Country
                }
            };
            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id
            });

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
