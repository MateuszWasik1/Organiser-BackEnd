using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Controllers;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestTasksNotesController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestTasksNotesController_Get_ShouldDispatch_GetTasksQuery()
        {
            //Arrange
            var controller = new TasksNotesController(dispatcher.Object);

            //Act
            controller.Get(new Guid(), 0 ,0);

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetTaskNoteQuery, GetTasksNotesViewModel> (It.IsAny<GetTaskNoteQuery>()), Times.Once);
        }

        [Test]
        public void TestTasksNotesController_AddTaskNote_ShouldDispatch_AddTaskNoteCommand()
        {
            //Arrange
            var controller = new TasksNotesController(dispatcher.Object);

            //Act
            controller.AddTaskNote(new TasksNotesAddViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<AddTaskNoteCommand>()), Times.Once);
        }

        [Test]
        public void TestTasksNotesController_DeleteTaskNote_ShouldDispatch_DeleteTaskNoteCommand()
        {
            //Arrange
            var controller = new TasksNotesController(dispatcher.Object);

            //Act
            controller.DeleteTaskNote(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteTaskNoteCommand>()), Times.Once);
        }
    }
}