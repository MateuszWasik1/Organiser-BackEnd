using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestSavingsController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestSavingsController_GetSaving_ShouldDispatch_GetSavingQuery()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.GetSaving(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetSavingQuery, SavingViewModel>(It.IsAny<GetSavingQuery>()), Times.Once);
        }

        [Test]
        public void TestSavingsController_GetSavings_ShouldDispatch_GetSavingsQuery()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.GetSavings(0, 0);

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetSavingsQuery, GetSavingsViewModel>(It.IsAny<GetSavingsQuery>()), Times.Once);
        }

        [Test]
        public void TestSavingsController_AddSaving_ShouldDispatch_AddSavingCommand()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.AddSaving(new SavingViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<AddSavingCommand>()), Times.Once);
        }

        [Test]
        public void TestSavingsController_UpdateSaving_ShouldDispatch_UpdateSavingCommand()
        {
            //Arrange
            var controller = new SavingsController(dispatcher.Object);

            //Act
            controller.UpdateSaving(new SavingViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<UpdateSavingCommand>()), Times.Once);
        }

        [Test]
        public void TestSavingsController_Delete_ShouldDispatch_DeleteSavingCommand()
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