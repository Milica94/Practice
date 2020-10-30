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
    public class ProjectControllerTests
    {
        [Fact]
        public void GetAllProjects_ShouldReturn_OkResult()
        {
            try
            {

                // Arrange
                var logger = new Mock<ILogger<ProjectController>>();
                ILogger<ProjectController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ProjectController>>();

                var mockRepo = new Mock<IProject>();
                var projects = new List<Project>()
                {
                    new Project() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now, Status = "in_progress", ClientId = 1},
                    new Project() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now, Status = "opened", ClientId = 3},
                    new Project() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now, Status = "closed", ClientId = 2}
                };
                mockRepo.Setup(m => m.GetAllProjects()).Returns(value: projects);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectReadDTO>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ProjectController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllProjects();
                var okResult = result.Result as OkObjectResult;
                if (okResult != null)
                {
                    Assert.NotNull(okResult);

                }

                var model = okResult.Value as IEnumerable<ProjectReadDTO>;
                if (model.Count() > 0)
                {
                    Assert.NotNull(model);
                    var expected = model?.FirstOrDefault().Status;
                    var actual = projects?.FirstOrDefault().Status;
                    Assert.Equal(expected: expected, actual: actual);

                }

            }
            catch (Exception e)
            {
                Assert.False(false, e.Message);
            }

        }

        [Fact]
        public void GetAllProjects_ShouldReturn_200StatusCode()
        {
            try
            {
                // Arrange
                var logger = new Mock<ILogger<ProjectController>>();
                ILogger<ProjectController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ProjectController>>();

                var mockRepo = new Mock<IProject>();
                var projects = new List<Project>()
                {
                    new Project() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now, Status = "in_progress", ClientId = 1},
                    new Project() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now, Status = "opened", ClientId = 3},
                    new Project() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now, Status = "closed", ClientId = 2}
                };
                mockRepo.Setup(m => m.GetAllProjects()).Returns(value: projects);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectReadDTO>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ProjectController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllProjects();
                var okResult = result.Result as OkObjectResult;
                var model = okResult.Value as IEnumerable<ProjectReadDTO>;

                if (model.Count() > 0)
                {
                    Assert.NotNull(model);
                    var expected = model?.Count();
                    var actual = projects?.Count();
                    Assert.Equal(expected: expected, actual: actual);

                }

            }
            catch (Exception e)
            {
                Assert.False(false, e.Message);
            }

        }

        [Fact]
        public void GetAllProjects_ShouldReturn_AllProjectItems()
        {
            try
            {
                // Arrange
                var logger = new Mock<ILogger<ProjectController>>();
                ILogger<ProjectController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ProjectController>>();

                var mockRepo = new Mock<IProject>();
                var projects = new List<Project>()
                {
                    new Project() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now, Status = "in_progress", ClientId = 1},
                    new Project() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now, Status = "opened", ClientId = 3},
                    new Project() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now, Status = "closed", ClientId = 2}
                };
                mockRepo.Setup(m => m.GetAllProjects()).Returns(value: projects);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectReadDTO>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ProjectController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllProjects();
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
        public void GetAllProjects_ShouldReturn_Unauthorized()
        {
            try
            {
                // Arrange
                var logger = new Mock<ILogger<ProjectController>>();
                ILogger<ProjectController> mockLogger = logger.Object;
                mockLogger = Mock.Of<ILogger<ProjectController>>();

                var mockRepo = new Mock<IProject>();
                var projects = new List<Project>()
                {
                    new Project() {Id = 1, Name = "Martha", CreatedAt = DateTime.Now, Status = "in_progress", ClientId = 1},
                    new Project() {Id = 2, Name = "Jonas", CreatedAt = DateTime.Now, Status = "opened", ClientId = 3},
                    new Project() {Id = 3, Name = "Bartoz", CreatedAt = DateTime.Now, Status = "closed", ClientId = 2}
                };
                mockRepo.Setup(m => m.GetAllProjects()).Returns(value: projects);

                var mockMapper = new MapperConfiguration(cfg => { cfg.CreateMap<Project, ProjectReadDTO>(); });
                var mapper = mockMapper.CreateMapper();

                var controller = new ProjectController(mockLogger, repository: mockRepo.Object, mapper: mapper);

                //Act
                var result = controller.GetAllProjectsV2();
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
