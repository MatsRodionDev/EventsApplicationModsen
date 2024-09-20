using EventsApplication.Application.Common.Interfaces.Providers;

namespace EventsApplication.Infrastructure.Auth
{
    public class CustomClaimsKeysProvider : ICustomClaimsKeysProvider
    {
        private const string USER_ID_CLAIM_KEY = "userId";
        private const string USER_ROLE_CLAIM_KEY = "roleeee";

        public string UserId => USER_ID_CLAIM_KEY;
        public string UserRole => USER_ROLE_CLAIM_KEY;
    }
}
