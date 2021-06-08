using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        public readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper Mapper)
        {
            _context = context;
            _mapper = Mapper;
        }

        

        async Task<MemberDto>  IUserRepository.GetMemberAsync(string username)
        {
            return await _context.Users
                        .Where(x => x.UserName == username)
                        //this select would bring all the columns including passwords
                        // .Select(user => new MemberDto
                        // {
                        //     Id = user.Id,
                        //     Username = user.UserName
                        // }).SingleOrDefaultAsync();

                        //this selects only the dto objects
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();
        }

        async Task<IEnumerable<MemberDto>> IUserRepository.GetMembersAsync()
        {
            return await _context.Users
                                 .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                 .ToListAsync();
        }

        async Task<AppUser> IUserRepository.GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        async Task<AppUser> IUserRepository.GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetUsersAsync()
        {
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        async Task<bool> IUserRepository.SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        void IUserRepository.Update(AppUser user)
        {
             _context.Entry(user).State = EntityState.Modified;
        }
    }
}