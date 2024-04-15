using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels.UserViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestUserController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestUserController_GetAllUsers_ShouldDispatch_GetAllUsersQuery()
        {
            //Arrange
            var controller = new UserController(dispatcher.Object);

            //Act
            controller.GetAllUsers(0, 0);

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetAllUsersQuery, GetUsersAdminViewModel>(It.IsAny<GetAllUsersQuery>()), Times.Once);
        }

        [Test]
        public void TestUserController_GetUserByAdmin_ShouldDispatch_GetUserByAdminQuery()
        {
            //Arrange
            var controller = new UserController(dispatcher.Object);

            //Act
            controller.GetUserByAdmin(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetUserByAdminQuery, UserAdminViewModel>(It.IsAny<GetUserByAdminQuery>()), Times.Once);
        }

        [Test]
        public void TestUserController_GetUser_ShouldDispatch_GetUserQuery()
        {
            //Arrange
            var controller = new UserController(dispatcher.Object);

            //Act
            controller.GetUser();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetUserQuery, UserViewModel>(It.IsAny<GetUserQuery>()), Times.Once);
        }

        [Test]
        public void TestUserController_SaveUser_ShouldDispatch_SaveUserCommand()
        {
            //Arrange
            var controller = new UserController(dispatcher.Object);

            //Act
            controller.SaveUser(new UserViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveUserCommand>()), Times.Once);
        }

        [Test]
        public void TestUserController_SaveUserByAdmin_ShouldDispatch_SaveUserByAdminCommand()
        {
            //Arrange
            var controller = new UserController(dispatcher.Object);

            //Act
            controller.SaveUserByAdmin(new UserAdminViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveUserByAdminCommand>()), Times.Once);
        }

        [Test]
        public void TestUserController_DeleteUser_ShouldDispatch_DeleteUserCommand()
        {
            //Arrange
            var controller = new UserController(dispatcher.Object);

            //Act
            controller.DeleteUser(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteUserCommand>()), Times.Once);
        }
    }
}