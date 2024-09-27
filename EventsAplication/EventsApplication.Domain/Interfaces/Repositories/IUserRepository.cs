using EventsApplication.Domain.Interfaces.Entity;
using EventsApplication.Domain.Models;

namespace EventsApplication.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<IEntity, User>
    {
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
