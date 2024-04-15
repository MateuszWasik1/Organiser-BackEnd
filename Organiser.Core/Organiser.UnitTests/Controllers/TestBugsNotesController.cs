using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Controllers;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestBugsNotesController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestBugsNotesController_GetBug_ShouldDispatch_GetBugNotesQuery()
        {
            //Arrange
            var controller = new BugsNotesController(dispatcher.Object);

            //Act
            controller.GetBugNotes(new Guid(), 0 ,0);

            //Assert

            dispatcher.Verify(x => x.DispatchQuery<GetBugNotesQuery, GetBugsNotesViewModel>(It.IsAny<GetBugNotesQuery>()), Times.Once);
        }

        [Test]
        public void TestBugsNotesController_SaveBugNote_ShouldDispatch_SaveBugNoteCommand()
        {
            //Arrange
            var controller = new BugsNotesController(dispatcher.Object);

            //Act
            controller.SaveBugNote(new BugsNotesViewModel());

            //Assert

            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveBugNoteCommand>()), Times.Once);
        }
    }
}
