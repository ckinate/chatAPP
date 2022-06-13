using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenservice;
        public AccountController(DataContext context, ITokenService tokenservice)
        {
            _context = context;
            _tokenservice = tokenservice;
        }

     [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register( RegisterDto registerDto)
        {
            if(await UserExist(registerDto.Username)) return BadRequest("UserName already taken");
         using var hmac = new HMACSHA512();
         var user = new AppUser{
               UserName = registerDto.Username,
               PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
               PasswordSalt = hmac.Key
         };
         _context.Add(user);
         await _context.SaveChangesAsync();
         return new UserDto{
             UserName = user.UserName,
             Token = _tokenservice.CreateToken(user)
         };
        }
         [HttpPost("userExist")]
        public async Task<bool> UserExist (string username){
         return await _context.Users.AnyAsync(x=>x.UserName.ToLower()==username.ToLower());
        }
          [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
        {
           var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower() == loginDto.UserName.ToLower());
           if(user == null) return Unauthorized("Invalid userName");
           using var hmac = new HMACSHA512(user.PasswordSalt);
           var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
           for( var i=0; i< computeHash.Length;i++){
            if(computeHash[i]!= user.PasswordHash[i] ) return Unauthorized("Invalid Password");
           }

            return new UserDto{
             UserName = user.UserName,
             Token = _tokenservice.CreateToken(user)
         };
        }
    }
}