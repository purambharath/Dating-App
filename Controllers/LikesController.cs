using System.Collections.Generic;
using System.Threading.Tasks;
using DTOs;
using Entities;
using Extentions;
using Helpers;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Controllers
{

    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;

        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var SourceUserId = User.GetUserId();

            var likedUser = await _userRepository.GetUserByUsernameAsync(username);

            var sourceUser = await _likesRepository.GetUserWithLikes(SourceUserId);

            if(likedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var UserLike = await _likesRepository.GetUserLike(SourceUserId,likedUser.Id);

            if(UserLike != null) return BadRequest("You already like this user");

            UserLike = new UserLike {
                SourceUserId = SourceUserId,
                LikedUserId = likedUser.Id
            };
          
          sourceUser.LikedUsers.Add(UserLike);

          if(await _userRepository.SaveAllAsync()) return Ok();
          

          return BadRequest("Failed to like user");
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {

              likesParams.UserId = User.GetUserId();
               var users = await _likesRepository.GetUserLikes(likesParams);

              Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
               return Ok(users);
        }
    }
}