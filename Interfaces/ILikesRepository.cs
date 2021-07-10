using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using DTOs;
using Entities;
using Helpers;

namespace Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int SourceUserId,int LikedUserId);


        Task<AppUser> GetUserWithLikes(int userId);

        Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams);
        
    }
}