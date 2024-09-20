namespace EventsApplication.Application.Common.Interfaces.Hashers
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string hashedPassword);
    }
}
