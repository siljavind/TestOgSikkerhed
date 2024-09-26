using Microsoft.AspNetCore.Authorization;

namespace BlazorApp1.Code
{
    public class ValidCPRRequirement : IAuthorizationRequirement
    {
        public ValidCPRRequirement() { }
    }
}
