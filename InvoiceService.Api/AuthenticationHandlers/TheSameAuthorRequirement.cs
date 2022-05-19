using InvoiceService.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InvoiceService.Api.AuthenticationHandlers
{
    public class TheSameAuthorRequirement : IAuthorizationRequirement // marked interface
    {

    }

    public class InvoiceAuthorizationHandler : AuthorizationHandler<TheSameAuthorRequirement, Invoice>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TheSameAuthorRequirement requirement, Invoice resource)
        {
            string lastname = context.User.FindFirstValue(ClaimTypes.GivenName);

            if (resource.Customer.LastName == lastname)
            {
                context.Succeed(requirement);
            }
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
