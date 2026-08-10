using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenkey = builder.Configuration["TokenKey"]
        ?? throw new InvalidOperationException("TokenKey is not configured.");
        
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenkey)),
           ValidateIssuer = false,
           ValidateAudience = false
        };
    });

var app = builder.Build();
app.UseCors(options =>
{
    options.AllowAnyMethod()
           .AllowAnyHeader()
           .WithOrigins("http://localhost:4200","https://localhost:4200");
});


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
