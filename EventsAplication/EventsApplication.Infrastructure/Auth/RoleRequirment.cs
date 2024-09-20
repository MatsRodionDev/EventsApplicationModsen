using EventsApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EventsApplication.Infrastructure.Auth
{
    public class RoleRequirment : IAuthorizationRequirement
    {
        public RoleRequirment(params Role[] roles)
        {
            Roles = roles;
        }

        public Role[] Roles { get; set; } = [];
    }
}
