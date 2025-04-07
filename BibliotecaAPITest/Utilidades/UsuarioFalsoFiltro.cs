using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BibliotecaAPITest.Utilidades
{
    public class UsuarioFalsoFiltro : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            context.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim("email", "alesiasera04@gmail.com")
            }, "prueba"));


            await next();



        }
    }
}
