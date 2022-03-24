namespace ALMS.Service
{
    public interface IPasswordHasherService : IServiceBase
    {
        string HashedPassword(string password);
    }
}