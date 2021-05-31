using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTOs;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext,IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _dataContext.Users
            .Where(u => u.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
             .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
           return  await _dataContext.Users
           .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
           .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
           return await _dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dataContext.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
           return await _dataContext.Users
           .Include(p => p.Photos)
           .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}