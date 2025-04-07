using System.Net;
using System.Security.Claims;
using System.Text.Json;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPITest.Utilidades;

namespace BibliotecaAPITest.PruebasIntegracion.Controllers.V1
{
    [TestClass]
    public class AutoresControllerPruebas : BasePruebas
    {
        private static readonly string url = "/api/v1/autores";
        private string nombreBD = Guid.NewGuid().ToString();

        [TestMethod]
        public async Task Get_Devuelve404_CuandoAutorNoExiste()
        {
            //Preparacion 
            var factory = ConstruirWebApplicationFactory(nombreBD);
            var cliente = factory.CreateClient();

            //Prueba
            var respuesta = await cliente.GetAsync($"{url}/1");

            //Verificacion
            var statusCode = respuesta.StatusCode;
            Assert.AreEqual(expected: HttpStatusCode.NotFound, actual: respuesta.StatusCode);

        }

        [TestMethod]
        public async Task Get_DevuelveAutor_CuandoAutorExiste()
        {
            //Preparacion 
            var context = ConstruirContext(nombreBD);
            context.Autores.Add(new Autor { Nombres = "Alesia", Apellidos = "Sera" });
            context.Autores.Add(new Autor { Nombres = "Pablo", Apellidos = "Granell" });
            await context.SaveChangesAsync();
            var factory = ConstruirWebApplicationFactory(nombreBD);
            var cliente = factory.CreateClient();

            //Prueba
            var respuesta = await cliente.GetAsync($"{url}/1");

            //Verificacion
            respuesta.EnsureSuccessStatusCode();
            var autor = JsonSerializer.Deserialize<AutorConLibrosDTO>(await respuesta.Content.ReadAsStringAsync(), jsonSerializerOptions)!;
            
            Assert.AreEqual(expected: 1, autor.Id);

        }

        [TestMethod]
        public async Task Post_Devuelve401_CuandoUsuarioNoEstaAutenticado() 
        {
            //Preparacion 
            var factory = ConstruirWebApplicationFactory(nombreBD, ignorarSeguridad: false);
            var cliente = factory.CreateClient();
            var autorCracionDTO = new AutorCreacionDTO
            {
                Nombres = "Alesia",
                Apellidos = "Sera",
                Identificacion = "123"
            };

            //Prueba
            var respuesta = await cliente.PostAsJsonAsync(url, autorCracionDTO);

            //Verificacion
            Assert.AreEqual(expected: HttpStatusCode.Unauthorized, actual: respuesta.StatusCode);
        }


        [TestMethod]
        public async Task Post_Devuelve403_CuandoUsuarioNoEsAdmin()
        {
            //Preparacion 
            var factory = ConstruirWebApplicationFactory(nombreBD, ignorarSeguridad: false);
            var token = await CrearUsuario(nombreBD, factory);
            var cliente = factory.CreateClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var autorCracionDTO = new AutorCreacionDTO
            {
                Nombres = "Alesia",
                Apellidos = "Sera",
                Identificacion = "123"
            };

            //Prueba
            var respuesta = await cliente.PostAsJsonAsync(url, autorCracionDTO);

            //Verificacion
            Assert.AreEqual(expected: HttpStatusCode.Forbidden, actual: respuesta.StatusCode);
        }


        [TestMethod]
        public async Task Post_Devuelve201_CuandoUsuarioEsAdmin()
        {
            //Preparacion 
            var factory = ConstruirWebApplicationFactory(nombreBD, ignorarSeguridad: false);
            var claims = new List<Claim> { adminClaim };
            var token = await CrearUsuario(nombreBD, factory, claims);
            var cliente = factory.CreateClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var autorCracionDTO = new AutorCreacionDTO
            {
                Nombres = "Alesia",
                Apellidos = "Sera",
                Identificacion = "123"
            };

            //Prueba
            var respuesta = await cliente.PostAsJsonAsync(url, autorCracionDTO);

            //Verificacion
            respuesta.EnsureSuccessStatusCode();
            Assert.AreEqual(expected: HttpStatusCode.Created, actual: respuesta.StatusCode);
        }

    }
}
