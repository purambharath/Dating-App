using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using DTOs;
using Entities;
using Extentions;
using Helpers;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _dataContext;
        public LikesRepository(DataContext dataContext)
        {
            _dataContext = dataContext;


        }

        public async Task<UserLike> GetUserLike(int SourceUserId, int LikedUserId)
        {
            return await _dataContext.Likes.FindAsync(SourceUserId, LikedUserId);
        }

        public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _dataContext.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _dataContext.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);

                users = likes.Select(like => like.LikedUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);

                users = likes.Select(like => like.SourseUser);

            }

            var LikedUsers = users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PageList<LikeDto>.CreateAsync(LikedUsers,likesParams.PageNumber,likesParams.PageSize);

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _dataContext.Users
                    .Include(x => x.LikedUsers)
                    .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}