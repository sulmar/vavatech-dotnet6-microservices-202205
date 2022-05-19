using Microsoft.AspNetCore.Authorization;

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
}
