using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NSubstitute;

namespace BibliotecaAPITest.PruebasUnitarias.Servicios
{
    [TestClass]
    public class ServiciosUsuariosPruebas
    {
        private UserManager<Usuario> userManager = null!;
        private IHttpContextAccessor contextAccessor = null!;
        private ServicioUsuarios servicioUsuarios = null!;

        [TestInitialize]
        public void Setup()
        {
            userManager = Substitute.For<UserManager<Usuario>>(
                Substitute.For<IUserStore<Usuario>>(), null, null, null, null, null, null, null, null);

            contextAccessor = Substitute.For<IHttpContextAccessor>();
            servicioUsuarios = new ServicioUsuarios(userManager, contextAccessor);
        }


        [TestMethod]
        public async Task ObtenerUsuario_RetornarNulo_CuandoNoHayClaimEmail() 
        {
            //Preparacion
            var httpContext = new DefaultHttpContext();
            contextAccessor.HttpContext.Returns(httpContext);

            //Prueba
            var usuario = await servicioUsuarios.ObtenerUsuario();

            //Verificacion
            Assert.IsNull(usuario);

        }


        [TestMethod]
        public async Task ObtenerUsuario_RetornarUsuario_CuandoHayClaimEmail()
        {
            //Preparacion
            var email = "emailprueba@gmail.com";
            var usuarioEsperado = new Usuario { Email = email };

            userManager.FindByEmailAsync(email)!.Returns(Task.FromResult(usuarioEsperado));

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("email", email)
            }));

            var httpContext = new DefaultHttpContext() { User = claims };
            contextAccessor.HttpContext.Returns(httpContext);

            //Prueba
            var usuario = await servicioUsuarios.ObtenerUsuario();

            //Verificacion
            Assert.IsNotNull(usuario);
            Assert.AreEqual(expected: email, actual: usuario.Email);

        }


        [TestMethod]
        public async Task ObtenerUsuario_RetornarNulo_CuandoUsuarioNoExiste()
        {
            //Preparacion
            var email = "emailprueba@gmail.com";
            var usuarioEsperado = new Usuario { Email = email };

            userManager.FindByEmailAsync(email)!.Returns(Task.FromResult<Usuario>(null!));

            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("email", email)
            }));

            var httpContext = new DefaultHttpContext() { User = claims };
            contextAccessor.HttpContext.Returns(httpContext);

            //Prueba
            var usuario = await servicioUsuarios.ObtenerUsuario();

            //Verificacion
            Assert.IsNull(usuario);

        }

    }
}
