using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaAPI.Validaciones;

namespace BibliotecaAPITest.PruebasUnitarias.Validaciones
{

    [TestClass]
    public class PrimeraLetraMayusculaAttributePruebas
    {
        [TestMethod]
        [DataRow("")]
        [DataRow("      ")]
        [DataRow(null)]
        public void IsValid_RetornaExitoso_SiValueEsVacio(string value)
        { 
            //Preparacion
            var primeraLetraMayusculaAttribute = new PrimeraLetraMayusculaAttribute();
            var validationContext = new ValidationContext(new object());

            //Prueba
            var resultado = primeraLetraMayusculaAttribute.GetValidationResult(value, validationContext);


            //Verificacion
            Assert.AreEqual(expected: ValidationResult.Success, actual: resultado);

        }

        [TestMethod]
        [DataRow("Alesia")]
        public void IsValid_RetornaExitoso_SiLaPrimeraLetraEsMayuscula(string value)
        {
            //Preparacion
            var primeraLetraMayusculaAttribute = new PrimeraLetraMayusculaAttribute();
            var validationContext = new ValidationContext(new object());

            //Prueba
            var resultado = primeraLetraMayusculaAttribute.GetValidationResult(value, validationContext);


            //Verificacion
            Assert.AreEqual(expected: ValidationResult.Success, actual: resultado);

        }

        [TestMethod]
        [DataRow("alesia")]
        public void IsValid_RetornaError_SiLaPrimeraLetraEsMinuscula(string value)
        {
            //Preparacion
            var primeraLetraMayusculaAttribute = new PrimeraLetraMayusculaAttribute();
            var validationContext = new ValidationContext(new object());

            //Prueba
            var resultado = primeraLetraMayusculaAttribute.GetValidationResult(value, validationContext);


            //Verificacion
            Assert.AreEqual(expected: "La primera letra debe ser mayuscula", actual: resultado!.ErrorMessage);

        }

    }
}
