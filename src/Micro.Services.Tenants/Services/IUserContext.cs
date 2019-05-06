namespace Micro.Services.Tenants.Services
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }

        int UserId { get; }
    }
}
