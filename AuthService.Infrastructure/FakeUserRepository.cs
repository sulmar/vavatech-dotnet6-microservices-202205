using AuthService.Domain;
using Bogus;
using Core.Intrastructure;

namespace AuthService.Infrastructure
{
    public class FakeUserRepository : FakeEntityRepository<int, User>, IUserRepository
    {
        public FakeUserRepository(Faker<User> faker) : base(faker)
        {
        }

        public Task<User> GetByUsernameAsync(string username)
        {            
            var user = _entities.SingleOrDefault(u => u.Value.UserName == username);

            return Task.FromResult(user.Value);
        }
    }
}
