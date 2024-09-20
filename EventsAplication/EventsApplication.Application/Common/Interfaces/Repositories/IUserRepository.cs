using EventsApplication.Application.Common.Interfaces.Entity;
using EventsApplication.Domain.Models;

namespace EventsApplication.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<IEntity, User>
    {
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
