using AutoMapper;
using EventsApplication.Domain.Interfaces.Repositories;
using EventsApplication.Domain.Models;
using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsApplication.Persistence.Repositories
{
    public class UserRepository : GenericRepository<UserEntity, User>, IUserRepository
    {
        public UserRepository(ApplicationDBContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var userEntity = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            return _mapper.Map<User>(userEntity);
        }
    }
}
