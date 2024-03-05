using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestRolesController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestRolesController_GetUserRoles_ShouldDispatch_GetUserRolesQuery()
        {
            //Arrange
            var controller = new RolesController(dispatcher.Object);

            //Act
            controller.GetUserRoles();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetUserRolesQuery, RolesViewModel>(It.IsAny<GetUserRolesQuery>()), Times.Once);
        }

        [Test]
        public void TestRolesController_GetIsUserSupport_ShouldDispatch_GetUserRolesQuery()
        {
            //Arrange
            var controller = new RolesController(dispatcher.Object);

            //Act
            controller.GetIsUserSupport();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetIsUserSupportQuery, bool>(It.IsAny<GetIsUserSupportQuery>()), Times.Once);
        }

        [Test]
        public void TestRolesController_GetIsUserAdmin_ShouldDispatch_GetIsUserAdminQuery()
        {
            //Arrange
            var controller = new RolesController(dispatcher.Object);

            //Act
            controller.GetIsUserAdmin();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetIsUserAdminQuery, bool>(It.IsAny<GetIsUserAdminQuery>()), Times.Once);
        }
    }
}