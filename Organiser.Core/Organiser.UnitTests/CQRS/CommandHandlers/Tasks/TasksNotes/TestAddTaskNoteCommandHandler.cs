using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
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
        public void TestAddTaskNoteCommandHandler_NoteIsEmpty_ShouldThrowTaskNotesTextRequiredException()
        {
            //Arrange
            var model = new TasksNotesAddViewModel()
            {
                TNGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TNNote = "",
                TNTGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd")
            };

            var command = new AddTaskNoteCommand() { Model = model };
            var handler = new AddTaskNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotesTextRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskNoteCommandHandler_NoteIsOver2000_ShouldThrowTaskNotesTextMax2000Exception()
        {
            //Arrange
            var model = new TasksNotesAddViewModel()
            {
                TNGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TNNote = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibu",
                TNTGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd")
            };

            var command = new AddTaskNoteCommand() { Model = model };
            var handler = new AddTaskNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotesTextMax2000Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskNoteCommandHandler_TaskNotFound_ShouldThrowTaskNotFoundException()
        {
            //Arrange
            var model = new TasksNotesAddViewModel() 
            { 
                TNGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"), 
                TNNote = "TaskNote", 
                TNTGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd") 
            };

            var command = new AddTaskNoteCommand() { Model = model };
            var handler = new AddTaskNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotFoundException>(() => handler.Handle(command));
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
