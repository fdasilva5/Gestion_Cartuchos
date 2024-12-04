using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using Models.DTOs;
using Services;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Controllers.Tests
{
    public class Asignar_ImpresoraControllerTests
    {
        private readonly Mock<IAsignar_Impresora_Service> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Asignar_ImpresoraController _controller;

        public Asignar_ImpresoraControllerTests()
        {
            _mockService = new Mock<IAsignar_Impresora_Service>();
            _mockMapper = new Mock<IMapper>();
            _controller = new Asignar_ImpresoraController(_mockService.Object, _mockMapper.Object, null);
        }

        // verifica que el método Create del controlador Asignar_ImpresoraController devuelve un resultado OkObjectResult cuando se le proporciona un DTO válido (Asignar_Impresora_DTO).
        [Fact]
        public async Task Create_ReturnsOkResult_WhenValidDTO()
        {
            // Arrange
            var asignarImpresoraDTO = new Asignar_Impresora_DTO
            {
                impresora_id = 1,
                cartucho_id = 1,
                fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                observaciones = "Test",
                impresora = new Impresora
                {
                    Id = 1,
                    modelo = "Modelo1",
                    marca = "Marca1",
                    oficina = new Oficina
                    {
                        nombre = "Oficina1"
                    },
                    oficina_id = 1
                },
                cartucho = new Cartucho
                {
                    Id = 1,
                    numero_serie = "12345",
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                    cantidad_recargas = 0,
                    modelo = new Modelo
                    {
                        modelo_cartuchos = "ModeloCartucho1",
                        marca = "Marca1",
                        stock = 10
                    },
                    modelo_id = 1,
                    estado = new Estado
                    {
                        nombre = "Estado1"
                    },
                    estado_id = 1
                }
            };

            _mockService.Setup(service => service.Create(It.IsAny<Asignar_Impresora_DTO>()))
                        .ReturnsAsync(asignarImpresoraDTO);

            var result = await _controller.Create(asignarImpresoraDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Asignar_Impresora_DTO>(okResult.Value);
            Assert.Equal(asignarImpresoraDTO.impresora_id, returnValue.impresora_id);
            Assert.Equal(asignarImpresoraDTO.cartucho_id, returnValue.cartucho_id);
        }

        [Fact] // Verifica que el método Create del controlador Asignar_ImpresoraController devuelve un resultado BadRequestObjectResult cuando se le proporciona un DTO con una impresora que ya tiene un cartucho asignado.

        public Task Desasignar_Cartucho_Test()
        {
            // Arrange
            var cartuchoId = 1;

            // Act
            var result = _controller.DesasignarCartucho(cartuchoId);

            // Assert
            _mockService.Verify(service => service.DesasignarCartucho(cartuchoId), Times.Once);
            return Task.CompletedTask;
        }
        


        
        [Fact]
        public async Task GetById_Test()
        {
            // Arrange
            var cartuchoId = 1;
            var asignarImpresoraDTO = new Asignar_Impresora_DTO
            {
                impresora_id = 1,
                cartucho_id = 1,
                fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                observaciones = "Test",
                impresora = new Impresora
                {
                    Id = 1,
                    modelo = "Modelo1",
                    marca = "Marca1",
                    oficina = new Oficina
                    {
                        nombre = "Oficina1"
                    },
                    oficina_id = 1
                },
                cartucho = new Cartucho
                {
                    Id = 1,
                    numero_serie = "12345",
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                    cantidad_recargas = 0,
                    modelo = new Modelo
                    {
                        modelo_cartuchos = "ModeloCartucho1",
                        marca = "Marca1",
                        stock = 10
                    },
                    modelo_id = 1,
                    estado = new Estado
                    {
                        nombre = "Estado1"
                    },
                    estado_id = 1
                }
            };

            // Simula el servicio para devolver el DTO simulado
            _mockService.Setup(service => service.GetById(cartuchoId)).ReturnsAsync(asignarImpresoraDTO);

            // Act
            var result = await _controller.GetById(cartuchoId);

            // Assert
            var returnValue = Assert.IsType<Asignar_Impresora_DTO>(result);
            Assert.Equal(asignarImpresoraDTO.impresora_id, returnValue.impresora_id);
            Assert.Equal(asignarImpresoraDTO.cartucho_id, returnValue.cartucho_id);

            // Verificar que el método del servicio se haya llamado una vez
            _mockService.Verify(service => service.GetById(cartuchoId), Times.Once);
        }

        [Fact] // Verifica que el método GetAll del controlador Asignar_ImpresoraController devuelve una lista de Asignar_Impresora_DTO.
        public async Task GetAll_Test()
        {           
            var data = new List<Asignar_Impresora_DTO>
            {
                new Asignar_Impresora_DTO
                {
                    impresora_id = 1,
                    cartucho_id = 1,
                    fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                    observaciones = "Test",
                    impresora = new Impresora
                    {
                        Id = 1,
                        modelo = "Modelo1",
                        marca = "Marca1",
                        oficina = new Oficina
                        {
                            nombre = "Oficina1"
                        },
                        oficina_id = 1
                    },
                    cartucho = new Cartucho
                    {
                        Id = 1,
                        numero_serie = "12345",
                        fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                        cantidad_recargas = 0,
                        modelo = new Modelo
                        {
                            modelo_cartuchos = "ModeloCartucho1",
                            marca = "Marca1",
                            stock = 10
                        },
                        modelo_id = 1,
                        estado = new Estado
                        {
                            nombre = "Estado1"
                        },
                        estado_id = 1
                    }
                },
                new Asignar_Impresora_DTO
                {
                    impresora_id = 2,
                    cartucho_id = 2,
                    fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                    observaciones = "Test2",
                    impresora = new Impresora
                    {
                        Id = 2,
                        modelo = "Modelo2",
                        marca = "Marca2",
                        oficina = new Oficina
                        {
                            nombre = "Oficina2"
                        },
                        oficina_id = 2
                    },
                    cartucho = new Cartucho
                    {
                        Id = 2,
                        numero_serie = "67890",
                        fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                        cantidad_recargas = 1,
                        modelo = new Modelo
                        {
                            modelo_cartuchos = "ModeloCartucho2",
                            marca = "Marca2",
                            stock = 20
                        },
                        modelo_id = 2,
                        estado = new Estado
                        {
                            nombre = "Estado2"
                        },
                        estado_id = 2
                    }
                }
            };

            _mockService.Setup(service => service.GetAll()).ReturnsAsync(data);

            var result = await _controller.GetAll();

            var returnValue = Assert.IsType<List<Asignar_Impresora_DTO>>(result);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal(1, returnValue[0].impresora_id);
            Assert.Equal(1, returnValue[0].cartucho_id);

            
            _mockService.Verify(service => service.GetAll(), Times.Once);
        }
    
        [Fact] // Verifica que el método Create del controlador Asignar_ImpresoraController devuelve un resultado BadRequestObjectResult cuando se le proporciona un DTO con una impresora que ya tiene un cartucho asignado.
        public async Task Create_ShouldThrowException_WhenImpresoraAlreadyHasCartucho()
        {
            // Arrange
            var asignarImpresoraDTO = new Asignar_Impresora_DTO
            {
                impresora_id = 1,
                cartucho_id = 1,
                fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                observaciones = "Test",
                impresora = new Impresora
                {
                    Id = 1,
                    modelo = "Modelo1",
                    marca = "Marca1",
                    oficina = new Oficina
                    {
                        nombre = "Oficina1"
                    },
                    oficina_id = 1
                },
                cartucho = new Cartucho
                {
                    Id = 1,
                    numero_serie = "12345",
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                    cantidad_recargas = 0,
                    modelo = new Modelo
                    {
                        modelo_cartuchos = "ModeloCartucho1",
                        marca = "Marca1",
                        stock = 10
                    },
                    modelo_id = 1,
                    estado = new Estado
                    {
                        nombre = "Estado1"
                    },
                    estado_id = 1
                }
            };

            // Simula la existencia de una asignación previa
            _mockService.Setup(service => service.Create(It.IsAny<Asignar_Impresora_DTO>()))
                        .ThrowsAsync(new InvalidOperationException("La impresora ya tiene un cartucho asignado."));

            // Act
            var result = await _controller.Create(asignarImpresoraDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new{message = "La impresora ya tiene un cartucho asignado."});
        }
    
      
        [Fact] // Verifica que el método Create del controlador Asignar_ImpresoraController devuelve un resultado BadRequestObjectResult cuando se le proporciona un DTO con datos faltantes.
        public async Task Create_ShouldReturnBadRequest_WhenDataIsMissing()
        {
            var asignarImpresoraDTO = new Asignar_Impresora_DTO
            {
                impresora_id = 0, 
                cartucho_id = 1,
                fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                observaciones = "Test",
                impresora = new Impresora
                {
                    Id = 1,
                    modelo = "Modelo1",
                    marca = "Marca1",
                    oficina = new Oficina
                    {
                        nombre = "Oficina1"
                    },
                    oficina_id = 1
                },
                cartucho = new Cartucho
                {
                    Id = 1,
                    numero_serie = "12345",
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                    cantidad_recargas = 0,
                    modelo = new Modelo
                    {
                        modelo_cartuchos = "ModeloCartucho1",
                        marca = "Marca1",
                        stock = 10
                    },
                    modelo_id = 1,
                    estado = new Estado
                    {
                        nombre = "Estado1"
                    },
                    estado_id = 1
                }
            };

            
            _mockService.Setup(service => service.Create(It.IsAny<Asignar_Impresora_DTO>()))
                        .ThrowsAsync(new ArgumentException("El ID de la impresora es obligatorio y debe ser mayor que cero."));

            var result = await _controller.Create(asignarImpresoraDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new { message = "El ID de la impresora es obligatorio y debe ser mayor que cero." });
        }

        [Fact] // Verifica que el método Create del controlador Asignar_ImpresoraController devuelve un resultado BadRequestObjectResult cuando se le proporciona un DTO con datos inválidos.
        public async Task Create_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            var asignarImpresoraDTO = new Asignar_Impresora_DTO
            {
                impresora_id = 1,
                cartucho_id = -1, 
                fecha_asignacion = DateOnly.FromDateTime(DateTime.Now),
                observaciones = "Test",
                impresora = new Impresora
                {
                    Id = 1,
                    modelo = "Modelo1",
                    marca = "Marca1",
                    oficina = new Oficina
                    {
                        nombre = "Oficina1"
                    },
                    oficina_id = 1
                },
                cartucho = new Cartucho
                {
                    Id = 1,
                    numero_serie = "12345",
                    fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                    cantidad_recargas = 0,
                    modelo = new Modelo
                    {
                        modelo_cartuchos = "ModeloCartucho1",
                        marca = "Marca1",
                        stock = 10
                    },
                    modelo_id = 1,
                    estado = new Estado
                    {
                        nombre = "Estado1"
                    },
                    estado_id = 1
                }
            };

            _mockService.Setup(service => service.Create(It.IsAny<Asignar_Impresora_DTO>()))
                        .ThrowsAsync(new ArgumentException("El ID del cartucho es obligatorio y debe ser mayor que cero."));

            var result = await _controller.Create(asignarImpresoraDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new { message = "El ID del cartucho es obligatorio y debe ser mayor que cero." });
        }
    }
}
