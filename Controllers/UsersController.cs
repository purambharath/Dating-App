
using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Interfaces;
using DTOs;
using AutoMapper;
using API.DTOs;
using System.Collections;
using System.Security.Claims;

namespace Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var Users = _context.Users.ToList();

            var users = await _userRepository.GetMembersAsync();

            // var usersToRetuen = _mapper.Map< IEnumerable< MemberDto>>(users);

           return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
          var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          var user = await _userRepository.GetUserByUsernameAsync(username);

          _mapper.Map(memberUpdateDto,user);


          _userRepository.Update(user);

          if(await _userRepository.SaveAllAsync())
          return NoContent();
   

           return BadRequest("Fail to update User");

        }


    }
}