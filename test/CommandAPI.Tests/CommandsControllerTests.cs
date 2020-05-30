using System;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using Xunit;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Dtos;

using Newtonsoft.Json.Serialization;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

    

        //**************************************************
        //*
        //GET   /api/commands Unit Tests
        //*
        //**************************************************

        //TEST 1.1
        [Fact]
        public void GetAllCommands_ReturnsZeroResources_WhenDBIsEmpty()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //TEST 1.2
        [Fact]
        public void GetAllCommands_ReturnsOneResource_WhenDBHasOneResource()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            var okResult = result.Result as OkObjectResult;

            var commands = okResult.Value as List<CommandReadDto>;

            Assert.Single(commands);
        }

        //TEST 1.3
        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

        }

        //TEST 1.4
        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        //**************************************************
        //*
        //GET   /api/commands/{id} Unit Tests
        //*
        //**************************************************

        //TEST 2.1        
        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //TEST 2.2
        [Fact]
        public void GetCommandByID_Returns200OK__WhenValidIDProvided()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //TEST 2.3
        [Fact]
        public void GetCommandByID_ReturnsCorrectResouceType_WhenValidIDProvided()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //**************************************************
        //*
        //POST   /api/commands/ Unit Tests
        //*
        //**************************************************

        //TEST 3.1
        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.CreateCommand(new CommandCreateDto { });

            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //TEST 3.2
        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.CreateCommand(new CommandCreateDto { });

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }


        //**************************************************
        //*
        //PUT   /api/commands/{id} Unit Tests
        //*
        //**************************************************

        //TEST 4.1
        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.UpdateCommand(1, new CommandUpdateDto { });

            //Assert
            Assert.IsType<NoContentResult>(result);
        }


        //TEST 4.2
        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.UpdateCommand(0, new CommandUpdateDto { });

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        //**************************************************
        //*
        //PATCH   /api/commands/{id} Unit Tests
        //*
        //**************************************************


        //TEST 5.1
        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.PartialCommandUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto> { });

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        //**************************************************
        //*
        //DELETE   /api/commands/{id} Unit Tests
        //*
        //**************************************************

        //TEST 6.1
        [Fact]
        public void DeleteCommand_Returns200OK_WhenValidResourceIDSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(1)).Returns(new Command { Id = 1, HowTo = "mock", Platform = "Mock", CommandLine = "Mock" });

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.DeleteCommand(1);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        //TEST 6.2
        [Fact]
        public void DeleteCommand_Returns_404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange 
            mockRepo.Setup(repo =>
              repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.DeleteCommand(0);

            //Assert
            Assert.IsType<NotFoundResult>(result);
            
        }

        //**************************************************
        //*
        //Private Support Methods
        //*
        //**************************************************




        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to genrate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}
