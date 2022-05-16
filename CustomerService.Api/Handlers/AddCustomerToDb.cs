using CustomerService.Api.Notifications;
using CustomerService.Domain;
using MediatR;

namespace CustomerService.Api.Handlers
{
    public class AddCustomerToDb : INotificationHandler<AddCustomerNotification>
    {

        private readonly ICustomerRepository customerRepository;

        public AddCustomerToDb(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task Handle(AddCustomerNotification notification, CancellationToken cancellationToken)
        {
            await customerRepository.AddAsync(notification.Customer);
        }
    }
}
