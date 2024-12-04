using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Models;
using Models.DTOs;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Services.Tests
{
    public class Asignar_ImpresoraServiceTest
    {
        private readonly Mock<Gestion_Cartuchos_Context> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Asignar_ImpresoraService _service;

        public Asignar_ImpresoraServiceTest()
        {
            _mockContext = new Mock<Gestion_Cartuchos_Context>();
            _mockMapper = new Mock<IMapper>();
            _service = new Asignar_ImpresoraService(_mockContext.Object, _mockMapper.Object);
        }

       
    }
}