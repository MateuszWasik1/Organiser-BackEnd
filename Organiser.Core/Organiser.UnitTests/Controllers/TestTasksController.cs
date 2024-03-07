using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestTasksController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestTasksController_Get_ShouldDispatch_GetTasksQuery()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.Get();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetTasksQuery, List < TasksViewModel >> (It.IsAny<GetTasksQuery>()), Times.Once);
        }

        [Test]
        public void TestTasksController_Save_ShouldDispatch_SaveTaskCommand()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.Save(new TasksViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveTaskCommand>()), Times.Once);
        }

        [Test]
        public void TestTasksController_Delete_ShouldDispatch_DeleteSavingCommand()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.Delete(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteTaskCommand>()), Times.Once);
        }
    }
}