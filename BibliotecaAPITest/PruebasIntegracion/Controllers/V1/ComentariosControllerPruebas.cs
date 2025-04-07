using System.Net;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPITest.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPITest.PruebasIntegracion.Controllers.V1
{
    [TestClass]
    public class ComentariosControllerPruebas : BasePruebas
    {

        private readonly string url = "/api/v1/libros/1/comentarios";
        private string nombreBD = Guid.NewGuid().ToString();

        private async Task CrearDataDePrueba()
        {
            var context = ConstruirContext(nombreBD);
            var autor = new Autor { Nombres = "Alesia", Apellidos = "Sera" };
            context.Add(autor);
            await context.SaveChangesAsync();

            var libro = new Libro { Titulo = "Titulo" };
            libro.Autores.Add(new AutorLibro { Autor = autor });
            context.Add(libro);
            await context.SaveChangesAsync();

        }

        [TestMethod]
        public async Task Delete_Devuelve204_CuandoUsuarioBorraSuPropioComentario()
        {

            //Preapracion
            await CrearDataDePrueba();
            var factory = ConstruirWebApplicationFactory(nombreBD, ignorarSeguridad: false);
            var token = await CrearUsuario(nombreBD, factory);
            var context = ConstruirContext(nombreBD);
            var usuario = await context.Users.FirstAsync();
            var comentario = new Comentario { Cuerpo = "contenido", UsuarioId = usuario!.Id, LibroId = 1 };
            context.Add(comentario);
            await context.SaveChangesAsync();
            var cliente = factory.CreateClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //Prueba
            var respuesta = await cliente.DeleteAsync($"{url}/{comentario.Id}");

            //Verificacion
            Assert.AreEqual(expected: HttpStatusCode.NoContent, actual: respuesta.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Devuelve403_CuandoUsuarioBorrarElComentarioDeOtro()
        {

            //Preapracion
            await CrearDataDePrueba();
            var factory = ConstruirWebApplicationFactory(nombreBD, ignorarSeguridad: false);
            var emailCreadorComentario = "creador@gmail.com";
            await CrearUsuario(nombreBD, factory, [], emailCreadorComentario);
            var context = ConstruirContext(nombreBD);
            var usuarioCreadorComentario = await context.Users.FirstAsync();
            var comentario = new Comentario { Cuerpo = "contenido", UsuarioId = usuarioCreadorComentario!.Id, LibroId = 1 };
            context.Add(comentario);
            await context.SaveChangesAsync();
            var tokenUsuarioDistinto = await CrearUsuario(nombreBD, factory, [], "distinto@gamil.com");
            var cliente = factory.CreateClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenUsuarioDistinto);

            //Prueba
            var respuesta = await cliente.DeleteAsync($"{url}/{comentario.Id}");

            //Verificacion
            Assert.AreEqual(expected: HttpStatusCode.Forbidden, actual: respuesta.StatusCode);
        }

    }
}
