using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.TasksNotes
{
    [TestFixture]
    public class TestAddTaskNoteCommandHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IUserContext>? user;

        private List<Cores.Entities.Tasks>? tasks;
        private List<Cores.Entities.TasksNotes>? tasksNotes = new List<Cores.Entities.TasksNotes>();

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd")
                }
            };

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.TasksNotes).Returns(tasksNotes.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.TasksNotes>())).Callback<Cores.Entities.TasksNotes>(taskNote => tasksNotes.Add(taskNote));

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestAddTaskNoteCommandHandler_TaskNotFound_ShouldThrowException()
        {
            //Arrange
            var model = new TasksNotesAddViewModel() { TNGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"), TNNote = "TaskNote", TNTGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd") };

            var command = new AddTaskNoteCommand() { Model = model };
            var handler = new AddTaskNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskNoteCommandHandler_TaskFound_ShouldAddNewTaskNote()
        {
            //Arrange
            var model = new TasksNotesAddViewModel() { TNGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"), TNNote = "TaskNote", TNTGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd") };

            var command = new AddTaskNoteCommand() { Model = model };
            var handler = new AddTaskNoteCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, tasks.Count);
            ClassicAssert.AreEqual(1, tasksNotes.Count);

            ClassicAssert.AreEqual(new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"), tasksNotes[0].TNGID);
            ClassicAssert.AreEqual(new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"), tasksNotes[0].TNTGID);
            ClassicAssert.AreEqual(1, tasksNotes[0].TNUID);
            ClassicAssert.AreEqual("TaskNote", tasksNotes[0].TNNote);
        }
    }
}
