using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Controllers;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestTasksController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestTasksController_GetTask_ShouldDispatch_GetTaskQuery()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.GetTask(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetTaskQuery, TaskViewModel> (It.IsAny<GetTaskQuery>()), Times.Once);
        }

        [Test]
        public void TestTasksController_GetTasks_ShouldDispatch_GetTasksQuery()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.GetTasks();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetTasksQuery, GetTasksViewModel>(It.IsAny<GetTasksQuery>()), Times.Once);
        }

        [Test]
        public void TestTasksController_AddTask_ShouldDispatch_AddTaskCommand()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.AddTask(new TaskViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<AddTaskCommand>()), Times.Once);
        }


        [Test]
        public void TestTasksController_UpdateTask_ShouldDispatch_UpdateTaskCommand()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.UpdateTask(new TaskViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<UpdateTaskCommand>()), Times.Once);
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

        [Test]
        public void TestTasksController_DeleteWithRelatedEntities_ShouldDispatch_DeleteTaskRelatedEntitiesCommand()
        {
            //Arrange
            var controller = new TasksController(dispatcher.Object);

            //Act
            controller.DeleteWithRelatedEntities(new TasksDeleteTaskRelatedEntitiesViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteTaskRelatedEntitiesCommand>()), Times.Once);
        }
    }
}