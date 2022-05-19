using Core.Domain;

namespace AuthService.Domain
{
    public interface IUserRepository : IEntityRepository<int, User>
    {
        Task<User> GetByUsernameAsync(string username);
    }
}
