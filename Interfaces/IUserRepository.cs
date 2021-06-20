using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using DTOs;
using Helpers;

namespace Interfaces
{
    public interface IUserRepository
    {

        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserGender(string username);

        Task<bool> SaveAllAsync();

    //    Task<MemberDto> GetMemberAsync(string username);
//   Task<PageList<MemberDto>> GetMemberAsync(UserParams userParams);
        
    }
}