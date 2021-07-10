using API.Entities;

namespace Entities
{
    public class UserLike
    {

        public AppUser SourseUser { get; set; }

        public int SourceUserId { get; set; }

        public AppUser LikedUser { get; set; }      

        public int LikedUserId { get; set; }
        
    }
}