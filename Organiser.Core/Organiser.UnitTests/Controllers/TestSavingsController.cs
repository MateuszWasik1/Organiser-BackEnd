using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestSavingsController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestRolesController_Get_ShouldDispatch_GetSavingsQuery()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.Get();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetSavingsQuery, List<SavingsViewModel>>(It.IsAny<GetSavingsQuery>()), Times.Once);
        }

        [Test]
        public void TestRolesController_Save_ShouldDispatch_SaveSavingCommand()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.Save(new SavingsViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveSavingCommand>()), Times.Once);
        }

        [Test]
        public void TestRolesController_Delete_ShouldDispatch_DeleteSavingCommand()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.Delete(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteSavingCommand>()), Times.Once);
        }
    }
}