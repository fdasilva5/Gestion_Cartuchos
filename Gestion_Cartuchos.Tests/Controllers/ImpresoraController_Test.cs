using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using Models.DTOs;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Controllers.Tests
{
    public class ImpresoraControllerTests
    {
        private readonly Mock<IImpresoraService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ImpresoraController _controller;

        public ImpresoraControllerTests()
        {
            _mockService = new Mock<IImpresoraService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ImpresoraController(_mockService.Object, _mockMapper.Object, null);
        }

        [Fact]
        public async Task GetAll_Test()
        {
            var impresoraDTO = new ImpresoraDTO
            {
                Id = 1,
                Modelo = "Modelo1",
                Marca = "Marca1",
                oficina_id = 1,
                oficina = new Oficina
                {
                    Id = 1,
                    nombre = "Oficina1"
                }
            };

            var impresora = new Impresora
            {
                Id = 1,
                modelo = "Modelo1",
                marca = "Marca1",
                oficina_id = 1,
                oficina = new Oficina
                {
                    Id = 1,
                    nombre = "Oficina1"
                },
                ImpresoraModelos = new List<ImpresoraModelo>
                {
                    new ImpresoraModelo
                    {
                        modelo_id = 1,
                        impresora_id = 1,
                        modelo = new Modelo
                        {
                            Id = 1,
                            modelo_cartuchos = "ModeloCartucho1",
                            marca = "Marca1",
                            stock = 10
                        },
                        impresora = new Impresora
                        {
                            Id = 1,
                            modelo = "Modelo1",
                            marca = "Marca1",
                            oficina_id = 1,
                            oficina = new Oficina
                            {
                                Id = 1,
                                nombre = "Oficina1"
                            }
                        }
                    }
                }
            };

            _mockService.Setup(service => service.GetAll()).ReturnsAsync(new List<ImpresoraDTO> { impresoraDTO });
            _mockMapper.Setup(mapper => mapper.Map<ImpresoraDTO>(impresora)).Returns(impresoraDTO);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ImpresoraDTO>>(okResult.Value);
            Assert.Single(returnValue);
        }
    
        [Fact]
        public async Task GetCompatibleModelos_Test()
        {
            var impresoraId = 1;
            var modeloDTO = new ModeloDTO
            {
                Id = 1,
                modelo_cartuchos = "ModeloCartucho1",
                marca = "Marca1",
                stock = 10
            };

            _mockService.Setup(service => service.GetCompatibleModelos(impresoraId)).ReturnsAsync(new List<ModeloDTO> { modeloDTO });

            var result = await _controller.GetCompatibleModelos(impresoraId);

            var returnValue = Assert.IsType<List<ModeloDTO>>(result);
            Assert.Single(returnValue);
        }
    
        [Fact]
        public async Task Create_Teste()
        {
            var impresoraDTO = new ImpresoraDTO
            {
                Id = 1,
                Modelo = "Modelo1",
                Marca = "Marca1",
                oficina_id = 1,
                oficina = new Oficina
                {
                    Id = 1,
                    nombre = "Oficina1"
                }
            };

            var impresora = new Impresora
            {
                Id = 1,
                modelo = "Modelo1",
                marca = "Marca1",
                oficina_id = 1,
                oficina = new Oficina
                {
                    Id = 1,
                    nombre = "Oficina1"
                },
                ImpresoraModelos = new List<ImpresoraModelo>
                {
                    new ImpresoraModelo
                    {
                        modelo_id = 1,
                        impresora_id = 1,
                        modelo = new Modelo
                        {
                            Id = 1,
                            modelo_cartuchos = "ModeloCartucho1",
                            marca = "Marca1",
                            stock = 10
                        },
                        impresora = new Impresora
                        {
                            Id = 1,
                            modelo = "Modelo1",
                            marca = "Marca1",
                            oficina_id = 1,
                            oficina = new Oficina
                            {
                                Id = 1,
                                nombre = "Oficina1"
                            }
                        }
                    }
                }
            };

            _mockService.Setup(service => service.Create(impresoraDTO)).ReturnsAsync(impresoraDTO);
            _mockMapper.Setup(mapper => mapper.Map<ImpresoraDTO>(impresora)).Returns(impresoraDTO);

            var result = await _controller.Create(impresoraDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ImpresoraDTO>(okResult.Value);
            Assert.Equal(impresoraDTO.Id, returnValue.Id);
        }

         [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDataIsMissing()
        {
            // Arrange
            var impresoraDTO = new ImpresoraDTO
            {
                Modelo = "",
                Marca = "Marca1",
                oficina_id = 1,
                oficina = new Oficina { Id = 1, nombre = "Oficina1" },
                CompatibleModeloIds = new List<int> { 1, 2 }
            };

            // Simula la existencia de datos faltantes
            _mockService.Setup(service => service.Create(It.IsAny<ImpresoraDTO>()))
                        .ThrowsAsync(new ArgumentException("El modelo es obligatorio."));

            // Act
            var result = await _controller.Create(impresoraDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new { message = "El modelo es obligatorio." });
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var impresoraDTO = new ImpresoraDTO
            {
                Modelo = "Modelo1",
                Marca = "Marca1",
                oficina_id = -1, // Invalid oficina_id
                oficina = new Oficina { Id = 1, nombre = "Oficina1" },
                CompatibleModeloIds = new List<int> { 1, 2 }
            };

            // Simula la existencia de datos inválidos
            _mockService.Setup(service => service.Create(It.IsAny<ImpresoraDTO>()))
                        .ThrowsAsync(new ArgumentException("El ID de la oficina es obligatorio y debe ser mayor que cero."));

            // Act
            var result = await _controller.Create(impresoraDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new { message = "El ID de la oficina es obligatorio y debe ser mayor que cero." });
        }
        
    }
}