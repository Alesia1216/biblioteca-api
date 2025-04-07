using Microsoft.AspNetCore.Authorization;

namespace BibliotecaAPITest.Utilidades
{
    public class AllowAnnonymousHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requirement in context.PendingRequirements)
            { 
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
