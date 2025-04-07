﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaAPI.Controllers.V1;
using BibliotecaAPI.DTOs;
using BibliotecaAPITest.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;

namespace BibliotecaAPITest.PruebasUnitarias.Controllers.V1
{
    [TestClass]
    public class LibrosControllerPruebas : BasePruebas
    {

        [TestMethod]
        public async Task Get_RetornaCeroLibros_CunadoNoHayLibros() 
        {
            //Preparacion
            var nombreBD = Guid.NewGuid().ToString();
            var context = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();
            IOutputCacheStore outputCacheStore = null!;

            var controller = new LibrosController(context, mapper, outputCacheStore);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var paginacionDTO = new PaginacionDTO(1, 1);

            //Prueba
            var respuesta = await controller.Get(paginacionDTO);

            //Verificacion
            Assert.AreEqual(expected: 0, actual: respuesta.Count());
        }
    }
}
