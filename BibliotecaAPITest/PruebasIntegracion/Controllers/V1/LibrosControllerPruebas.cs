using System.Net;
using BibliotecaAPI.DTOs;
using BibliotecaAPITest.Utilidades;

namespace BibliotecaAPITest.PruebasIntegracion.Controllers.V1
{
    [TestClass]
    public class LibrosControllerPruebas : BasePruebas
    {
        private readonly string url = " /api/v1/libros";
        private string nombreBD = Guid.NewGuid().ToString();

        [TestMethod]
        public async Task Post_Devuelve400_CuandoAutoresIdsEsVacio()
        {
            //Preparacion
            var factory = ConstruirWebApplicationFactory(nombreBD);
            var cliente = factory.CreateClient();
            var libroCreacionDTO = new LibroCreacionDTO { Titulo = "Titulo" };

            //Prueba
            var respuesta = await cliente.PostAsJsonAsync(url, libroCreacionDTO);

            //Verificacion
            Assert.AreEqual(expected: HttpStatusCode.BadRequest, actual: respuesta.StatusCode);
        }

    }
}
