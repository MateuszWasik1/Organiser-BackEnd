using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Controllers;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestTasksSubTasksController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestTasksSubTasksController_GetSubTasks_ShouldDispatch_GetSubTasksQuery()
        {
            //Arrange
            var controller = new TasksSubTasksController(dispatcher.Object);

            //Act
            controller.GetSubTasks(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetSubTasksQuery, List<TasksSubTasksViewModel>> (It.IsAny<GetSubTasksQuery>()), Times.Once);
        }

        [Test]
        public void TestTasksSubTasksController_AddTaskSubTask_ShouldDispatch_AddTaskSubTaskCommand()
        {
            //Arrange
            var controller = new TasksSubTasksController(dispatcher.Object);

            //Act
            controller.AddTaskSubTask(new TasksAddSubTaskViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<AddTaskSubTaskCommand>()), Times.Once);
        }

        [Test]
        public void TestTasksSubTasksController_ChangeSubTaskStatus_ShouldDispatch_ChangeTaskSubTaskStatusCommand()
        {
            //Arrange
            var controller = new TasksSubTasksController(dispatcher.Object);

            //Act
            controller.ChangeSubTaskStatus(new TasksSubTasksChangeStatusViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<ChangeTaskSubTaskStatusCommand>()), Times.Once);
        }

        [Test]
        public void TestTasksSubTasksController_DeleteTaskSubTask_ShouldDispatch_DeleteTaskSubTaskCommand()
        {
            //Arrange
            var controller = new TasksSubTasksController(dispatcher.Object);

            //Act
            controller.DeleteTaskSubTask(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteTaskSubTaskCommand>()), Times.Once);
        }
    }
}