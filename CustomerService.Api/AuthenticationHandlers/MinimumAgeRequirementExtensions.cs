using Microsoft.AspNetCore.Authorization;

namespace CustomerService.Api.AuthenticationHandlers
{
    public static class MinimumAgeRequirementExtensions
    {
        public static AuthorizationPolicyBuilder RequireAge(this AuthorizationPolicyBuilder builder, int minimumAge)
        {
            builder.Requirements.Add(new MinimumAgeRequirement(minimumAge));

            return builder;
        }
    }
}
