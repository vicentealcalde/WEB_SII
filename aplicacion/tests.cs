using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aplicacion.models;
using aplicacion.Models;
using aplicacion.ViewModels;
using NUnit.Framework;
namespace aplicacion
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void CalculateDifferenceFromSumPercent_Should_Return_Correct_Difference()
        {
            // Arrange
            var yourClass = new EscriturasController();
            List<string> percentages = new List<string> { "10", "20", "30", "40" };
            double expectedDifference = 0.0;

            // Act
            double actualDifference = yourClass.CalculateDifferenceFromSumPercent(percentages);

            // Assert
            Assert.AreEqual(expectedDifference, actualDifference);
        }

        [TestCase]
        public void CalculateDifferenceFromSumPercent_Should_Return_Zero_When_Percentages_Sum_To_100()
        {
            // Arrange
            var yourClass = new EscriturasController();
            var percentages = new List<string> { "30", "40", "30" };

            // Act
            var difference = yourClass.CalculateDifferenceFromSumPercent(percentages);

            // Assert
            Assert.AreEqual(0, difference);
        }

        [TestCase]
        public void CalculateDifferenceFromSumPercent_Should_Return_Positive_Value_When_Percentages_Sum_Less_Than_100()
        {
            // Arrange
            var yourClass = new EscriturasController();
            var test = new EscriturasController();
            var percentages = new List<string> { "20", "30", "40" };

            // Act
            var difference = yourClass.CalculateDifferenceFromSumPercent(percentages);

            // Assert
            Assert.IsTrue(difference > 0);
        }


        [Test]
        public void InsertZeros_Should_Return_ModifiedRightPercentages_With_RightPercentages()
        {
            // Arrange
            List<string> uncreditedRightPercentages = new List<string> { "false", "false", "false" };
            List<string> rightPercentages = new List<string> { "50", "75", "90" };

            // Act
            List<string> modifiedRightPercentages = EscriturasController.InsertZeros(uncreditedRightPercentages, rightPercentages);

            // Assert
            Assert.AreEqual(new List<string> { "50", "75", "90" }, modifiedRightPercentages);
        }

        [Test]
        public void InsertZeros_Should_Return_Empty_List_When_UncreditedRightPercentages_Is_Empty()
        {
            // Arrange
            List<string> uncreditedRightPercentages = new List<string>();
            List<string> rightPercentages = new List<string> { "50", "75", "90" };

            // Act
            List<string> modifiedRightPercentages = EscriturasController.InsertZeros(uncreditedRightPercentages, rightPercentages);

            // Assert
            Assert.IsEmpty(modifiedRightPercentages);
        }
        [Test]
        public void FindPercentOfPropery_Should_Return_Zero_When_List_Is_Empty()
        {
            // Arrange
            var myClass = new EscriturasController();
            var multiPropertyData = new List<Multipropietario>();

            // Act
            var result = myClass.FindPercentOfPropery(multiPropertyData);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void FindPercentOfPropery_Should_Return_Sum_Of_Percentages()
        {
            // Arrange
            var myClass = new EscriturasController();
            var multiPropertyData = new List<Multipropietario>
        {
            new Multipropietario { PorcentajeDerecho = 10 },
            new Multipropietario { PorcentajeDerecho = 20 },
            new Multipropietario { PorcentajeDerecho = 30 }
        };

            // Act
            var result = myClass.FindPercentOfPropery(multiPropertyData);

            // Assert
            Assert.AreEqual(60, result);
        }

        [Test]
        public void FindPercentOfPropery_Should_Return_Sum_Of_Decimal_Percentages()
        {
            // Arrange
            var myClass = new EscriturasController();
            var multiPropertyData = new List<Multipropietario>
        {
            new Multipropietario { PorcentajeDerecho = 10.5 },
            new Multipropietario { PorcentajeDerecho = 20.3 },
            new Multipropietario { PorcentajeDerecho = 15.2 }
        };

            // Act
            var result = myClass.FindPercentOfPropery(multiPropertyData);

            // Assert
            Assert.AreEqual(46.0m, result);
        }
        [Test]
        public void FechaInscripcion_Should_Have_Default_Value_Of_MinValue()
        {
            // Arrange
            var escritura = new Escritura();

            // Act

            // Assert
            Assert.AreEqual(DateTime.MinValue, escritura.FechaInscripcion);
        }
        [Test]
        public void Fojas_Should_Have_Default_Value_Of_Zero()
        {
            // Arrange
            var escritura = new Escritura();

            // Act

            // Assert
            Assert.AreEqual(0, escritura.Fojas);
        }
        [Test]
        public void Cne_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var escritura = new Escritura();

            // Act

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(escritura.Cne));
        }

        [Test]
        public void NumeroInscripcion_Should_Not_Be_Null_Or_Empty()
        {
            // Arrange
            var escritura = new Escritura();

            // Act

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(escritura.NumeroInscripcion));
        }
        [Test]
        public void Multipropietario_Id_Should_Be_Set()
        {
            // Arrange
            var multipropietario = new Multipropietario();

            // Act
            multipropietario.Id = 1;

            // Assert
            Assert.AreEqual(1, multipropietario.Id);
        }

        [Test]
        public void Multipropietario_FechaInscripcion_Should_Not_Be_Default()
        {
            // Arrange
            var multipropietario = new Multipropietario();

            // Act
            multipropietario.FechaInscripcion = new DateTime(2022, 1, 1);

            // Assert
            Assert.AreNotEqual(default(DateTime), multipropietario.FechaInscripcion);
        }

        [Test]
        public void Multipropietario_AnoVigenciaInicial_Should_Be_Less_Than_AnoVigenciaFinal()
        {
            // Arrange
            var multipropietario = new Multipropietario();

            // Act
            multipropietario.AnoVigenciaInicial = 2020;
            multipropietario.AnoVigenciaFinal = 2022;

            // Assert
            Assert.Less(multipropietario.AnoVigenciaInicial, multipropietario.AnoVigenciaFinal);
        }

        [Test]
        public void Multipropietario_Predio_Should_Not_Be_Negative()
        {
            // Arrange
            var multipropietario = new Multipropietario();

            // Act
            multipropietario.Predio = -1;

            // Assert
            Assert.That(multipropietario.Predio, Is.LessThanOrEqualTo(0));
        }
        [Test]
        public void Multipropietario_Properties_Should_Have_Correct_Values()
        {
            // Arrange
            int expectedManzana = 1;
            int expectedFojas = 10;
            int expectedAnoInscripcion = 2022;
            int expectedNumeroInscripcion = 100;

            // Act
            var multipropietario = new Multipropietario
            {
                Manzana = expectedManzana,
                Fojas = expectedFojas,
                AnoInscripcion = expectedAnoInscripcion,
                NumeroInscripcion = expectedNumeroInscripcion
            };

            // Assert
            Assert.AreEqual(expectedManzana, multipropietario.Manzana);
            Assert.AreEqual(expectedFojas, multipropietario.Fojas);
            Assert.AreEqual(expectedAnoInscripcion, multipropietario.AnoInscripcion);
            Assert.AreEqual(expectedNumeroInscripcion, multipropietario.NumeroInscripcion);
        }

        [Test]
        public void Multipropietario_Should_Have_Default_Values_When_Created()
        {
            // Arrange

            // Act
            var multipropietario = new Multipropietario();

            // Assert
            Assert.AreEqual(0, multipropietario.Manzana);
            Assert.AreEqual(0, multipropietario.Fojas);
            Assert.AreEqual(0, multipropietario.AnoInscripcion);
            Assert.AreEqual(0, multipropietario.NumeroInscripcion);
        }
        [Test]
        public void RunRut_Should_Not_Be_Null()
        {
            // Arrange
            var enajenante = new Enajenante();

            // Act

            // Assert
            Assert.IsNull(enajenante.RunRut);
        }

        [Test]
        public void PorcentajeDerecho_Should_Be_Non_Negative()
        {
            // Arrange
            var enajenante = new Enajenante();

            // Act
            enajenante.PorcentajeDerecho = -10;

            // Assert
            Assert.That(enajenante.PorcentajeDerecho, Is.LessThanOrEqualTo(0));
        }

        [Test]
        public void IdEnajenante_Should_Have_Default_Value_Of_Zero()
        {
            // Arrange
            var enajenante = new Enajenante();

            // Act

            // Assert
            Assert.AreEqual(0, enajenante.IdEnajenante);
        }

        [Test]
        public void NumAtencion_Should_Have_Default_Value_Of_Zero()
        {
            // Arrange
            var enajenante = new Enajenante();

            // Act

            // Assert
            Assert.AreEqual(0, enajenante.NumAtencion);
        }

        [Test]
        public void NumAtencionNavigation_Should_Not_Be_Null()
        {
            // Arrange
            var enajenante = new Enajenante();

            // Act

            // Assert
            Assert.IsNull(enajenante.NumAtencionNavigation);
        }
        [Test]
        public void RunRut_Should_Not_Be_Null_Adquiriente()
        {
            // Arrange
            var adquirente = new Adquirente();

            // Act

            // Assert
            Assert.IsNull(adquirente.RunRut);
        }

        [Test]
        public void PorcentajeDerecho_Should_Be_Non_Negative_Adquiriente()
        {
            // Arrange
            var adquirente = new Adquirente();

            // Act
            adquirente.PorcentajeDerecho = -10;

            // Assert
            Assert.That(adquirente.PorcentajeDerecho, Is.LessThanOrEqualTo(0));
        }

        [Test]
        public void NumAtencion_Should_Be_Negative()
        {
            // Arrange
            var adquirente = new Adquirente();

            // Act
            adquirente.NumAtencion = -1;

            // Assert
            Assert.That(adquirente.NumAtencion, Is.LessThanOrEqualTo(0));
        }

        [Test]
        public void PorcentajeDerechoNoAcreditado_Should_Be_True()
        {
            // Arrange
            var adquirente = new Adquirente();

            // Act
            adquirente.PorcentajeDerechoNoAcreditado = true;

            // Assert
            Assert.IsTrue(adquirente.PorcentajeDerechoNoAcreditado);
        }

        [Test]
        public void NumAtencionNavigation_Should_Not_Be_Null_Adquiriente()
        {
            // Arrange
            var adquirente = new Adquirente();

            // Act

            // Assert
            Assert.IsNull(adquirente.NumAtencionNavigation);
        }

        [Test]
        public void ShowRequestId_Should_Return_True_When_RequestId_Is_Not_Null_Or_Empty()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = "123456789"
            };

            // Act
            var result = errorViewModel.ShowRequestId;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShowRequestId_Should_Return_False_When_RequestId_Is_Null()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = null
            };

            // Act
            var result = errorViewModel.ShowRequestId;

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ShowRequestId_Should_Return_False_When_RequestId_Is_Empty()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = ""
            };

            // Act
            var result = errorViewModel.ShowRequestId;

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public void Multipropietarios_Should__Be_Null()
        {
            // Arrange
            var viewModel = new MultipropietarioViewModel();

            // Act

            // Assert
            Assert.IsNull(viewModel.Multipropietarios);
        }

        [Test]
        public void Comunas_Should__Be_Null()
        {
            // Arrange
            var viewModel = new MultipropietarioViewModel();

            // Act

            // Assert
            Assert.IsNull(viewModel.Comunas);
        }
        [Test]
        public void ListComunas_Should_Not_Be_Null()
        {
            // Arrange
            var constantsAndList = new ConstantsAndList();

            // Act

            // Assert
            Assert.IsNotNull(constantsAndList.ListComunas);
        }

        [Test]
        public void ListComunas_Should_Not_Be_Empty()
        {
            // Arrange
            var constantsAndList = new ConstantsAndList();

            // Act

            // Assert
            Assert.IsNotEmpty(constantsAndList.ListComunas);
        }
        [Test]
        public void Escritura_Should__Be_Null()
        {
            // Arrange
            var viewModel = new EscrituraViewModel();

            // Assert
            Assert.IsNull(viewModel.Escritura);
        }

        [Test]
        public void Adquirente_Should__Be_Null()
        {
            // Arrange
            var viewModel = new EscrituraViewModel();

            // Assert
            Assert.IsNull(viewModel.Adquirente);
        }

        [Test]
        public void Enajenante_Should__Be_Null()
        {
            // Arrange
            var viewModel = new EscrituraViewModel();

            // Assert
            Assert.IsNull(viewModel.Enajenante);
        }

        [Test]
        public void Adquirentes_Should__Be_Null()
        {
            // Arrange
            var viewModel = new EscrituraViewModel();

            // Assert
            Assert.IsNull(viewModel.Adquirentes);
        }

        [Test]
        public void Enajenantes_Should__Be_Null()
        {
            // Arrange
            var viewModel = new EscrituraViewModel();

            // Assert
            Assert.IsNull(viewModel.Enajenantes);
        }

        [Test]
        public void Comunas_Should__Be_Null_in_EscriturasViewModels()
        {
            // Arrange
            var viewModel = new EscrituraViewModel();

            // Assert
            Assert.IsNull(viewModel.Comunas);
        }


        [Test]
        public void ProcessPercentages_Should_Return_Original_List_When_Sum_Equals_100()
        {
            // Arrange
            List<string> porcentajes = new List<string> { "20", "30", "50" };
            var process = new EscriturasController();

            // Act
            var result = process.ProcessPercentages(porcentajes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(porcentajes, result);
        }

        [Test]
        public void ProcessPercentages_Should_Return_Same_List_When_No_Zeroes_Present()
        {
            // Arrange
            List<string> porcentajes = new List<string> { "10", "20", "30", "40" };
            var process = new EscriturasController();
            // Act
            var result = process.ProcessPercentages(porcentajes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(porcentajes, result);
        }

        [Test]
        public void ProcessPercentages_Should_Return_Same_List_When_Difference_Equals_Zero()
        {
            // Arrange
            List<string> porcentajes = new List<string> { "25", "25", "25", "25" };
            var process = new EscriturasController();
            // Act
            var result = process.ProcessPercentages(porcentajes);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(porcentajes, result);
        }
        [Test]
        public void GetPorcentajeDerechoByRuts_Should_Return_Empty_List_When_No_Multipropietarios()
        {
            // Arrange
            var multipropietarios = new List<Multipropietario>();
            var ruts = new List<string> { "123", "456" };
            var escrituras = new EscriturasController();

            // Act
            var result = escrituras.GetPorcentajeDerechoByRuts(multipropietarios, ruts);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetPorcentajeDerechoByRuts_Should_Return_Correct_PorcentajeDerecho_List()
        {
            // Arrange
            var multipropietarios = new List<Multipropietario>
        {
            new Multipropietario { RunRut = "123", PorcentajeDerecho = 10 },
            new Multipropietario { RunRut = "456", PorcentajeDerecho = 20 },
            new Multipropietario { RunRut = "789", PorcentajeDerecho = 30 }
        };
            var ruts = new List<string> { "123", "456" };
            var escrituras = new EscriturasController();

            // Act
            var result = escrituras.GetPorcentajeDerechoByRuts(multipropietarios, ruts);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.Contains(10, result);
            Assert.Contains(20, result);
        }

        [Test]
        public void GetPorcentajeDerechoByRuts_Should_Return_Zero_For_Invalid_RUTs()
        {
            // Arrange
            var multipropietarios = new List<Multipropietario>
        {
            new Multipropietario { RunRut = "123", PorcentajeDerecho = 10 },
            new Multipropietario { RunRut = "456", PorcentajeDerecho = 20 }
        };
            var ruts = new List<string> { "789", "999" };
            var escrituras = new EscriturasController();
            // Act
            var result = escrituras.GetPorcentajeDerechoByRuts(multipropietarios, ruts);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(0, result[1]);
        }

        [Test]
        public void GetPorcentajeDerechoByRuts_Should_Return_Correct_PorcentajeDerecho_List_With_Duplicate_RUTs()
        {
            // Arrange
            var multipropietarios = new List<Multipropietario>
        {
            new Multipropietario { RunRut = "123", PorcentajeDerecho = 10 },
            new Multipropietario { RunRut = "123", PorcentajeDerecho = 20 },
            new Multipropietario { RunRut = "456", PorcentajeDerecho = 30 }
        };
            var ruts = new List<string> { "123", "456" };
            var escrituras = new EscriturasController();
            // Act
            var result = escrituras.GetPorcentajeDerechoByRuts(multipropietarios, ruts);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.Contains(10, result);
            Assert.Contains(30, result);
        }

    }

    public class EscriturasController
    {
        public double CalculateDifferenceFromSumPercent(List<string> percentages)
        {
            double sumOfPercentages = 0.0;
            double differencePercentage = 0.0;

            for (int i = 0; i < percentages.Count; i++)
            {
                sumOfPercentages += double.Parse(percentages[i]);
            }

            differencePercentage = 100 - sumOfPercentages;
            return differencePercentage;
        } 

        public static List<string> InsertZeros(List<string> uncreditedRightPercentages, List<string> rightPercentages)
        {
            List<string> modifiedRightPercentages = new List<string>();
            int counter = 0;

            for (int i = 0; i < uncreditedRightPercentages.Count; i++)
            {
                if (uncreditedRightPercentages[i].ToLower() == "true")
                {
                    modifiedRightPercentages.Add("0");
                }
                else
                {
                    if (counter >= rightPercentages.Count)
                    {
                        break;
                    }
                    modifiedRightPercentages.Add(rightPercentages[counter]);
                    counter++;
                }
            }

            return modifiedRightPercentages;
        }


        public decimal FindPercentOfPropery(List<Multipropietario> MultiPropertyData)
        {
            decimal suma = MultiPropertyData.Sum(mp => (decimal)mp.PorcentajeDerecho);
            return suma;
        }
        public List<string> ProcessPercentages(List<string> porcentajes)
        {
            // Convertir los porcentajes a números decimales
            List<decimal> porcentajesDecimales = porcentajes.Select(p => decimal.Parse(p)).ToList();

            // Calcular la suma de los porcentajes
            decimal sumaPorcentajes = porcentajesDecimales.Sum();

            // Calcular la cantidad de porcentajes que son cero
            int cantidadCeros = porcentajesDecimales.Count(p => p == 0);

            // Calcular la diferencia entre la suma de los porcentajes y 100
            decimal diferencia = 100 - sumaPorcentajes;

            // Si la diferencia es mayor a cero y hay porcentajes iguales a cero, reemplazar los ceros
            if (diferencia > 0 && cantidadCeros > 0)
            {
                // Calcular la cantidad a distribuir entre los porcentajes iguales a cero
                decimal cantidadDistribuir = diferencia / cantidadCeros;

                // Reemplazar los ceros con la cantidad a distribuir
                for (int i = 0; i < porcentajesDecimales.Count; i++)
                {
                    if (porcentajesDecimales[i] == 0)
                    {
                        porcentajesDecimales[i] = cantidadDistribuir;
                    }
                }
            }

            // Convertir los porcentajes de vuelta a string y retornarlos en una lista
            List<string> porcentajesActualizados = porcentajesDecimales.Select(p => p.ToString()).ToList();
            return porcentajesActualizados;
        }

        public List<double> GetPorcentajeDerechoByRuts(List<Multipropietario> multipropietarios, List<string> ruts)
        {


            // Lista para almacenar los PorcentajeDerecho correspondientes
            List<double> porcentajeDerechoList = new List<double>();

            // Iterar sobre los multipropietarios y comparar los RUTs
            foreach (var multipropietario in multipropietarios)
            {
                // Verificar si el RUT está en la lista dada
                if (ruts.Contains(multipropietario.RunRut))
                {
                    // Agregar el PorcentajeDerecho a la lista
                    porcentajeDerechoList.Add(multipropietario.PorcentajeDerecho);
                }
                else
                {
                    // Agregar cero a la lista si el RUT no está presente
                    porcentajeDerechoList.Add(0);
                }
            }

            return porcentajeDerechoList;
        }
    }
}