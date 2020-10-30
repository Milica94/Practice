using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TAP.Controllers;
using TAP.Data;
using TAP.Dtos;
using TAP.Models;
using Xunit;

namespace TAP.Test
{
    public class ClientControllerTests
    {

        [Fact]
        public void GetAllClients_ShouldReturn_OkResult()
        {
            try
            {

                // Arrange
                var logger = new Mock<ILogger<ClientController>>();
                ILogger<ClientController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ClientController>>();

                var mockRepo = new Mock<IClient>();
                var clients = new List<Client>()
                {
                    new Client() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now},
                    new Client() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now},
                    new Client() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now}
                };
                mockRepo.Setup(m => m.GetAllClients()).Returns(value: clients);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Client, ClientReadDto>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ClientController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllClients();
                var okResult = result.Result as OkObjectResult;
                if (okResult != null)
                {
                    Assert.NotNull(okResult);

                }

                var model = okResult.Value as IEnumerable<ClientReadDto>;
                if (model.Count() > 0)
                {
                    Assert.NotNull(model);
                    var expected = model?.FirstOrDefault().Id;
                    var actual = clients?.FirstOrDefault().Id;
                    Assert.Equal(expected: expected, actual: actual);

                }

            }
            catch (Exception e)
            {
                Assert.False(false, e.Message);
            }

        }

        [Fact]
        public void GetAllClients_ShouldReturn_200StatusCode()
        {
            try
            {
                // Arrange
                var logger = new Mock<ILogger<ClientController>>();
                ILogger<ClientController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ClientController>>();

                var mockRepo = new Mock<IClient>();
                var clients = new List<Client>()
                {
                    new Client() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now},
                    new Client() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now},
                    new Client() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now}
                };
                mockRepo.Setup(m => m.GetAllClients()).Returns(value: clients);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Client, ClientReadDto>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ClientController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllClients();
                var okResult = result.Result as OkObjectResult;
                var model = okResult.Value as IEnumerable<ClientReadDto>;

                if (model.Count() > 0)
                {
                    Assert.NotNull(model);
                    var expected = model?.Count();
                    var actual = clients?.Count();
                    Assert.Equal(expected: expected, actual: actual);

                }

            }
            catch (Exception e)
            {
                Assert.False(false, e.Message);
            }

        }

        [Fact]
        public void GetAllClients_ShouldReturn_AllClientItems()
        {
            try
            {
                // Arrange
                var logger = new Mock<ILogger<ClientController>>();
                ILogger<ClientController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ClientController>>();

                var mockRepo = new Mock<IClient>();
                var clients = new List<Client>()
                {
                    new Client() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now},
                    new Client() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now},
                    new Client() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now}
                };
                mockRepo.Setup(m => m.GetAllClients()).Returns(value: clients);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Client, ClientReadDto>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ClientController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllClients();
                var okResult = result.Result as OkObjectResult;
                if (okResult != null)
                {
                    Assert.Equal(200, okResult.StatusCode);

                }

            }
            catch (Exception e)
            {
                Assert.False(false, e.Message);
            }

        }

        [Fact]
        public void GetAllClients_ShouldReturn_Unauthorized()
        {
            try
            {
                // Arrange
                var logger = new Mock<ILogger<ClientController>>();
                ILogger<ClientController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ClientController>>();

                var mockRepo = new Mock<IClient>();
                var clients = new List<Client>()
                {
                    new Client() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now},
                    new Client() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now},
                    new Client() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now}
                };
                mockRepo.Setup(m => m.GetAllClients()).Returns(value: clients);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Client, ClientReadDto>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ClientController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllClientsV2();
                var okResult = result.Result as UnauthorizedObjectResult;
                if (okResult != null)
                {
                    Assert.Equal(401, okResult.StatusCode);

                }

            }
            catch (Exception e)
            {
                Assert.False(false, e.Message);
            }

        }

    }
}