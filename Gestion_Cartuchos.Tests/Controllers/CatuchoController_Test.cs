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
    public class CartuchoControllerTests
    {
        private readonly Mock<ICartuchoService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CartuchoController _controller;

        public CartuchoControllerTests()
        {
            _mockService = new Mock<ICartuchoService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CartuchoController(_mockService.Object, _mockMapper.Object, null);
        }

        [Fact]
        public async Task GetAll_Test()
        {
            // Arrange
            var cartuchoDTO = new CartuchoDTO
            {
                Id = 1,
                numero_serie = "12345",
                fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                cantidad_recargas = 0,
                modelo = new Modelo
                {
                    Id = 1,
                    modelo_cartuchos = "ModeloCartucho1",
                    marca = "Marca1",
                    stock = 10
                },
                estado = new Estado
                {
                    Id = 1,
                    nombre = "Disponible"
                },
                modelo_id = 1,
                estado_id = 1
            };

            var cartucho = new Cartucho
            {
                Id = 1,
                numero_serie = "12345",
                fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                cantidad_recargas = 0,
                modelo = new Modelo
                {
                    Id = 1,
                    modelo_cartuchos = "ModeloCartucho1",
                    marca = "Marca1",
                    stock = 10
                },
                estado = new Estado
                {
                    Id = 1,
                    nombre = "Disponible"
                },
                modelo_id = 1,
                estado_id = 1
            };

            _mockService.Setup(service => service.GetAll()).ReturnsAsync(new List<CartuchoDTO> { cartuchoDTO });
            _mockMapper.Setup(mapper => mapper.Map<CartuchoDTO>(cartucho)).Returns(cartuchoDTO);

            var result = await _controller.GetAll();

            
            var returnValue = Assert.IsType<List<CartuchoDTO>>(result);
            Assert.Single(returnValue);
        }
    
        [Fact]

        public async Task GetById_Test()
        {
            // Arrange
            var cartuchoId = 1;
            var cartuchoDTO = new CartuchoDTO
            {
                Id = 1,
                numero_serie = "12345",
                fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                cantidad_recargas = 0,
                modelo = new Modelo
                {
                    Id = 1,
                    modelo_cartuchos = "ModeloCartucho1",
                    marca = "Marca1",
                    stock = 10
                },
                estado = new Estado
                {
                    Id = 1,
                    nombre = "Disponible"
                },
                modelo_id = 1,
                estado_id = 1
            };

            var cartucho = new Cartucho
            {
                Id = 1,
                numero_serie = "12345",
                fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                cantidad_recargas = 0,
                modelo = new Modelo
                {
                    Id = 1,
                    modelo_cartuchos = "ModeloCartucho1",
                    marca = "Marca1",
                    stock = 10
                },
                estado = new Estado
                {
                    Id = 1,
                    nombre = "Disponible"
                },
                modelo_id = 1,
                estado_id = 1
            };

            _mockService.Setup(service => service.GetById(cartuchoId)).ReturnsAsync(cartuchoDTO);
            _mockMapper.Setup(mapper => mapper.Map<CartuchoDTO>(cartucho)).Returns(cartuchoDTO);

            var result = await _controller.GetById(cartuchoId);

             var returnValue = Assert.IsType<CartuchoDTO>(result);
            Assert.Equal(cartuchoDTO.Id, returnValue.Id);
        }
    
    
        [Fact]
        public async Task Create_Test()
        {
            // Arrange
            var cartuchoDTO = new CartuchoDTO
            {
                Id = 1,
                numero_serie = "12345",
                fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                cantidad_recargas = 0,
                modelo = new Modelo
                {
                    Id = 1,
                    modelo_cartuchos = "ModeloCartucho1",
                    marca = "Marca1",
                    stock = 10
                },
                estado = new Estado
                {
                    Id = 1,
                    nombre = "Disponible"
                },
                modelo_id = 1,
                estado_id = 1
            };

            var cartucho = new Cartucho
            {
                Id = 1,
                numero_serie = "12345",
                fecha_alta = DateOnly.FromDateTime(DateTime.Now),
                cantidad_recargas = 0,
                modelo = new Modelo
                {
                    Id = 1,
                    modelo_cartuchos = "ModeloCartucho1",
                    marca = "Marca1",
                    stock = 10
                },
                estado = new Estado
                {
                    Id = 1,
                    nombre = "Disponible"
                },
                modelo_id = 1,
                estado_id = 1
            };

            _mockService.Setup(service => service.Create(cartuchoDTO)).ReturnsAsync(cartuchoDTO);
            _mockMapper.Setup(mapper => mapper.Map<CartuchoDTO>(cartucho)).Returns(cartuchoDTO);

            var result = await _controller.Create(cartuchoDTO);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CartuchoDTO>(okResult.Value);
            Assert.Equal(cartuchoDTO.Id, returnValue.Id);
        }
    

         [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDataIsMissing()
        {
            // Arrange
            var cartuchoDTO = new CartuchoDTO
            {
                numero_serie = "",// numero_serie is missing
                modelo_id = 1,
                estado_id = 1,
                estado = new Estado { Id = 1, nombre = "Disponible" },
                modelo = new Modelo { Id = 1, modelo_cartuchos = "Modelo1", marca = "Marca1", stock = 10 }
            };

            // Simula la existencia de datos faltantes
            _mockService.Setup(service => service.Create(It.IsAny<CartuchoDTO>()))
                        .ThrowsAsync(new ArgumentException("El número de serie es obligatorio."));

            // Act
            var result = await _controller.Create(cartuchoDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new { message = "El número de serie es obligatorio." });
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var cartuchoDTO = new CartuchoDTO
            {
                numero_serie = "12345",
                modelo_id = -1, // Invalid modelo_id
                estado_id = 1,
                estado = new Estado { Id = 1, nombre = "Disponible" },
                modelo = new Modelo { Id = 1, modelo_cartuchos = "Modelo1", marca = "Marca1", stock = 10 }
            };

            // Simula la existencia de datos inválidos
            _mockService.Setup(service => service.Create(It.IsAny<CartuchoDTO>()))
                        .ThrowsAsync(new ArgumentException("El ID del modelo es obligatorio y debe ser mayor que cero."));

            // Act
            var result = await _controller.Create(cartuchoDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().BeEquivalentTo(new { message = "El ID del modelo es obligatorio y debe ser mayor que cero." });
        }


        [Fact]
        public async Task UpdateEstado_Test()
        {
            // Arrange
            var cartuchoId = 1;

            // Act
            var result = await _controller.UpdateEstado(cartuchoId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockService.Verify(service => service.ChangeEstadoToEnRecarga(cartuchoId), Times.Once);
        }
    }
}