namespace EventsApplication.Application.Common
{
    public static class CacheKeysProvider
    {
        private const string REFRESH_KEY = "refresh:";

        public static string GetRefreshTokenKey(Guid userId)
        {
            return REFRESH_KEY + userId.ToString();
        }
    }
}
