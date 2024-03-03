using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Accounts.Commands;
using Organiser.Core.CQRS.Resources.Accounts.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels.AccountsViewModel;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestAccountsController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestAccountsController_Register_ShouldDispatch_RegisterUserCommand()
        {
            //Arrange
            var controller = new AccountsController(dispatcher.Object);

            //Act
            controller.Register(new RegisterViewModel());

            //Assert

            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<RegisterUserCommand>()), Times.Once);
        }

        [Test]
        public void TestAccountsController_Login_ShouldDispatch_LoginQuery()
        {
            //Arrange
            var controller = new AccountsController(dispatcher.Object);

            //Act
            controller.Login("", "");

            //Assert

            dispatcher.Verify(x => x.DispatchQuery<LoginQuery, string>(It.IsAny<LoginQuery>()), Times.Once);
        }
    }
}
