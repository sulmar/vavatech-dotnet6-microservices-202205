using CustomerService.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CustomerService.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [HttpGet("ping")]
        public string Ping()
        {
            return "Pong";
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            var customers = await customerRepository.GetAsync();

            return customers;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var customer = await customerRepository.GetAsync(id);

            //if (customer==null)
            //{
            //    return new NotFoundResult();
            //}

            // return new OkObjectResult(customer);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

       


    }
}
