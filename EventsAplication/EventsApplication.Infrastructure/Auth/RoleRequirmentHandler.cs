using EventsApplication.Application.Common.Interfaces.Providers;
using EventsApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApplication.Infrastructure.Auth
{
    public class RoleRequirmentHandler : AuthorizationHandler<RoleRequirment>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RoleRequirmentHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            RoleRequirment requirement)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var customKeysClaimsKeyProvider = scope.ServiceProvider.GetRequiredService<ICustomClaimsKeysProvider>();

            

            var userRole = context.User.Claims.FirstOrDefault(
                 c => c.Type == customKeysClaimsKeyProvider.UserRole);

            if (userRole == null || !Enum.TryParse<Role>(userRole.Value, out var role))
            {
                return Task.CompletedTask;  
            }

            if (requirement.Roles.Contains(role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
