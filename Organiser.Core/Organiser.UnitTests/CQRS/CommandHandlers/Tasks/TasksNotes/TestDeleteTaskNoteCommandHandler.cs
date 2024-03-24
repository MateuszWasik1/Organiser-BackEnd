using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.TasksNotes
{
    [TestFixture]
    public class TestDeleteTaskNoteCommandHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.TasksNotes>? tasksNotes = new List<Cores.Entities.TasksNotes>();

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            tasksNotes = new List<Cores.Entities.TasksNotes>()
            {
                new Cores.Entities.TasksNotes()
                {
                    TNID = 1,
                    TNGID = new Guid("98dacc1d-7bee-4635-9c4c-9404a4af80dd")
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 2,
                    TNGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd")
                },
            };

            context.Setup(x => x.TasksNotes).Returns(tasksNotes.AsQueryable());

            context.Setup(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>())).Callback<Cores.Entities.TasksNotes>(taskNote => tasksNotes.Remove(taskNote));
        }

        [Test]
        public void TestDeleteTaskNoteCommandHandler_TaskNoteNotFound_ShouldThrowTaskNotesNotFoundException()
        {
            //Arrange
            var command = new DeleteTaskNoteCommand() { TNGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd") };
            var handler = new DeleteTaskNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotesNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteTaskNoteCommandHandler_TaskNoteFound_ShouldDeleteTaskNote()
        {
            //Arrange
            var command = new DeleteTaskNoteCommand() { TNGID = tasksNotes[1].TNGID };
            var handler = new DeleteTaskNoteCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, tasksNotes.Count);
            ClassicAssert.IsFalse(tasksNotes.Any(x => x.TNGID == new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd")));
        }
    }
}
