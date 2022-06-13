using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

         [HttpGet]
     public ActionResult<IEnumerable<AppUser>> getUsers()
     {
         return _context.Users.ToList();
     }
       [HttpGet("{id}")]
     public ActionResult<AppUser> getUser(int Id)
     {
       return _context.Users.Find(Id);
     }

    }
}