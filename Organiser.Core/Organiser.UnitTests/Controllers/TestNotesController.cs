using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Controllers;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestNotesController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestNotesController_GetNotes_ShouldDispatch_GetNotesQuery()
        {
            //Arrange
            var controller = new NotesController(dispatcher.Object);

            //Act
            controller.GetNotes(0, 0);

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetNotesQuery, GetNotesViewModel>(It.IsAny<GetNotesQuery>()), Times.Once);
        }

        [Test]
        public void TestNotesController_AddNote_ShouldDispatch_AddNoteCommand()
        {
            //Arrange
            var controller = new NotesController(dispatcher.Object);

            //Act
            controller.AddNote(new NotesAddViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<AddNoteCommand>()), Times.Once);
        }

        [Test]
        public void TestNotesController_UpdateNote_ShouldDispatch_UpdateNoteCommand()
        {
            //Arrange
            var controller = new NotesController(dispatcher.Object);

            //Act
            controller.UpdateNote(new NotesUpdateViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<UpdateNoteCommand>()), Times.Once);
        }

        [Test]
        public void TestNotesController_DeleteNote_ShouldDispatch_DeleteNoteCommand()
        {
            //Arrange
            var controller = new NotesController(dispatcher.Object);

            //Act
            controller.DeleteNote(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteNoteCommand>()), Times.Once);
        }
    }
}