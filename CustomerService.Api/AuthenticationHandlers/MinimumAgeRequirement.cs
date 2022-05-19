using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CustomerService.Api.AuthenticationHandlers
{
    public class MinimumAgeRequirement : IAuthorizationRequirement // marked interface
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }


    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            DateTime dateOfBirth = Convert.ToDateTime(context.User.FindFirstValue(ClaimTypes.DateOfBirth));

            int age = DateTime.Today.Year - dateOfBirth.Year;

            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }

    public static class MinimumAgeRequirementExtensions
    {
        public static AuthorizationPolicyBuilder RequireAge(this AuthorizationPolicyBuilder builder, int minimumAge)
        {
            builder.Requirements.Add(new MinimumAgeRequirement(minimumAge));

            return builder;
        }
    }
}
