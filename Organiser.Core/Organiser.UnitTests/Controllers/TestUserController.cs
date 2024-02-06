﻿using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Cores.Context;
using Organiser.Cores.Controllers;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestUserController
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IUserContext>? user;
        private Mock<IMapper>? mapper;

        private List<User>? users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();
            mapper = new Mock<IMapper>();

            users = new List<User>() 
            { 
                new User()
                {
                    UID = 1,
                    UFirstName = "UFirstName1",
                    ULastName = "ULastName1",
                    UUserName = "UUserName1",
                    UEmail = "UEmail1",
                    UPhone = "UPhone1",
                },
                new User()
                {
                    UID = 2,
                    UFirstName = "UFirstName2",
                    ULastName = "ULastName2",
                    UUserName = "UUserName2",
                    UEmail = "UEmail2",
                    UPhone = "UPhone2",
                },
            };

            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<User>())).Callback<User>(user => 
            { 
                var currentUser = users.FirstOrDefault(x => x.UID == user.UID);

                users[users.FindIndex(x => x.UID == currentUser.UID)] = user;
            });

            user.Setup(x => x.UID).Returns(1);

            mapper.Setup(m => m.Map<User, UserViewModel>(It.IsAny<User>())).Returns(new UserViewModel() {
                UFirstName = users[0].UFirstName,
                ULastName = users[0].ULastName,
                UUserName = users[0].UUserName,
                UEmail = users[0].UEmail,
                UPhone = users[0].UPhone,
            });
        }

        //Get
        [Test]
        public void TestUserController_GetUser_UserNotFound_ShouldThrowException()
        {
            //Arrange
            var controller = new UserController(context.Object, user.Object, mapper.Object);
            user.Setup(x => x.UID).Returns(223);

            //Act
            //Assert
            Assert.Throws<Exception>(() => controller.GetUser());
        }

        [Test]
        public void TestTasksController_GetUser_ShouldReturnUser()
        {
            //Arrange
            var controller = new UserController(context.Object, user.Object, mapper.Object);

            //Act
            var result = controller.GetUser();

            //Assert
            ClassicAssert.AreEqual("UFirstName1", result.UFirstName);
            ClassicAssert.AreEqual("ULastName1", result.ULastName);
            ClassicAssert.AreEqual("UUserName1", result.UUserName);
            ClassicAssert.AreEqual("UEmail1", result.UEmail);
            ClassicAssert.AreEqual("UPhone1", result.UPhone);
        }

        //Post
        [Test]
        public void TestUserController_SaveUser_UserNotFound_ShouldThrowException()
        {
            //Arrange
            var controller = new UserController(context.Object, user.Object, mapper.Object);
            user.Setup(x => x.UID).Returns(22);

            //Act
            //Assert
            Assert.Throws<Exception>(() => controller.SaveUser(new UserViewModel()));
        }

        [Test]
        public void TestTasksController_SaveUser_UserIsFound_ShouldModifyUser()
        {
            //Arrange
            var controller = new UserController(context.Object, user.Object, mapper.Object);

            var model = new UserViewModel()
            {
                UFirstName = "UFirstName3",
                ULastName = "ULastName3",
                UUserName = "UUserName3",
                UEmail = "UEmail3",
                UPhone = "UPhone3",
            };

            //Act
            controller.SaveUser(model);

            //Assert
            ClassicAssert.AreEqual(2, users?.Count);
            ClassicAssert.AreEqual("UFirstName3", users?[0].UFirstName);
            ClassicAssert.AreEqual("ULastName3", users?[0].ULastName);
            ClassicAssert.AreEqual("UUserName3", users?[0].UUserName);
            ClassicAssert.AreEqual("UEmail3", users?[0].UEmail);
            ClassicAssert.AreEqual("UPhone3", users?[0].UPhone);
        }
    }
}