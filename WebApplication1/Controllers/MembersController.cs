using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class MembersController(AppDbContext appContext) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await appContext.Users.ToListAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await appContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
    }
}
