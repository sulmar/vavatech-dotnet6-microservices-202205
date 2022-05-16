using Bogus;
using Core.Intrastructure;
using CustomerService.Domain;

namespace CustomerService.Infrastructure
{
    public class FakeCustomerRepository : FakeEntityRepository<int, Customer>, ICustomerRepository
    {
        public FakeCustomerRepository(Faker<Customer> faker) : base(faker)
        {
        }

        public Task<IEnumerable<Customer>> GetByAdress(string city, string country)
        {
            throw new NotImplementedException();
        }
    }
}