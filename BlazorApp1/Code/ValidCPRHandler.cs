using BlazorApp1.Data.DbContexts;
using Microsoft.AspNetCore.Authorization;

namespace BlazorApp1.Code
{
    public class ValidCPRHandler : AuthorizationHandler<ValidCPRRequirement>
    {
        private readonly ToDoDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidCPRHandler(ToDoDbContext toDoDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = toDoDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidCPRRequirement requirement)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }

    }
}
