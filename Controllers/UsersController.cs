
using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
   
    public class UsersController : BaseApiController
    {


        private readonly DataContext _context;
        public UsersController(DataContext context)
        {

            _context = context;

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            // var Users = _context.Users.ToList();
            return  await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async  Task< ActionResult<AppUser>> GetUser(int id)
        {
             
            return await _context.Users.FindAsync(id);
        }

    }
}