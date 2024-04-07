using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.Tasks
{
    [TestFixture]
    public class TestDeleteWithRelatedEntitiesCommandHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Tasks>? tasks;
        private List<Cores.Entities.TasksNotes>? tasksNotes;
        private List<Cores.Entities.TasksSubTasks>? tasksSubTasks;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TLocalization = "Lokalizacja 1",
                    TName = "Nazwa 1",
                    TBudget = 2050,
                    TStatus = TaskEnum.NotStarted,
                    TTime = new DateTime(2023, 12, 4, 21, 30, 0)
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f6dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TLocalization = "Lokalizacja 2",
                    TName = "Nazwa 2",
                    TBudget = 2060,
                    TStatus = TaskEnum.OnGoing,
                    TTime = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Cores.Entities.Tasks()
                {
                    TID = 3,
                    TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TLocalization = "Lokalizacja 3",
                    TName = "Nazwa 3",
                    TBudget = 2070,
                    TStatus = TaskEnum.Done,
                    TTime = new DateTime(2023, 12, 6, 21, 30, 0)
                },
            };

            tasksNotes = new List<Cores.Entities.TasksNotes>()
            {
                new Cores.Entities.TasksNotes()
                {
                    TNID = 1,
                    TNGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = tasks[1].TGID,
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 2,
                    TNGID = new Guid("f10acc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = tasks[1].TGID,
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 3,
                    TNGID = new Guid("f11acc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = tasks[2].TGID,
                },
            };

            tasksSubTasks = new List<Cores.Entities.TasksSubTasks>()
            {
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 1,
                    TSTGID = new Guid("f12acc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TSTTGID = tasks[1].TGID,
                },
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 2,
                    TSTGID = new Guid("f13acc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TSTTGID = tasks[1].TGID,
                },
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 3,
                    TSTGID = new Guid("f14acc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TSTTGID = tasks[2].TGID,
                },
            };

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.TasksNotes).Returns(tasksNotes.AsQueryable());
            context.Setup(x => x.TasksSubTasks).Returns(tasksSubTasks.AsQueryable());

            context.Setup(x => x.DeleteTask(It.IsAny<Cores.Entities.Tasks>())).Callback<Cores.Entities.Tasks>(task => tasks.Remove(task));
            context.Setup(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>())).Callback<Cores.Entities.TasksNotes>(taskNotes => tasksNotes.Remove(taskNotes));
            context.Setup(x => x.DeleteTaskSubTask(It.IsAny<Cores.Entities.TasksSubTasks>())).Callback<Cores.Entities.TasksSubTasks>(taskSubTasks => tasksSubTasks.Remove(taskSubTasks));
        }

        [Test]
        public void TestDeleteWithRelatedEntitiesCommandHandler_DeleteTask_TaskNotFound_ShouldThrowTaskNotFoundException()
        {
            //Arrange
            var model = new TasksDeleteTaskRelatedEntitiesViewModel()
            {
                TGID = Guid.NewGuid(),
                DeleteTaskSubTasks = false,
                DeleteTaskNotes = false,
            };

            var command = new DeleteTaskRelatedEntitiesCommand() { Model = model };
            var handler = new DeleteTaskRelatedEntitiesCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteWithRelatedEntitiesCommandHandler_DeleteTask_TaskIsFound_DeleteTaskNotesAndTask_ShouldDeleteTask_AndRelatedTaskNotesAndTask()
        {
            //Arrange
            var model = new TasksDeleteTaskRelatedEntitiesViewModel()
            {
                TGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                DeleteTaskSubTasks = false,
                DeleteTaskNotes = true,
            };

            var command = new DeleteTaskRelatedEntitiesCommand() { Model = model };
            var handler = new DeleteTaskRelatedEntitiesCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(2, tasks.Count);
            ClassicAssert.AreEqual(1, tasksNotes.Count);
            ClassicAssert.AreEqual(3, tasksSubTasks.Count);

            ClassicAssert.IsFalse(tasks.Any(x => x.TGID == new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksNotes.Any(x => x.TNGID == new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksNotes.Any(x => x.TNGID == new Guid("f10acc1d-7bee-4635-9c4c-9404a4af80dd")));

            context.Verify(x => x.DeleteTask(It.IsAny<Cores.Entities.Tasks>()), Times.Once);
            context.Verify(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>()), Times.Exactly(2));
            context.Verify(x => x.DeleteTaskSubTask(It.IsAny<Cores.Entities.TasksSubTasks>()), Times.Never);
        }

        [Test]
        public void TestDeleteWithRelatedEntitiesCommandHandler_DeleteTask_TaskIsFound_DeleteTaskSubTasksAndTask_ShouldDeleteTask_AndRelatedTaskSubTasksAndTask()
        {
            //Arrange
            var model = new TasksDeleteTaskRelatedEntitiesViewModel()
            {
                TGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                DeleteTaskSubTasks = true,
                DeleteTaskNotes = false,
            };

            var command = new DeleteTaskRelatedEntitiesCommand() { Model = model };
            var handler = new DeleteTaskRelatedEntitiesCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(2, tasks.Count);
            ClassicAssert.AreEqual(3, tasksNotes.Count);
            ClassicAssert.AreEqual(1, tasksSubTasks.Count);

            ClassicAssert.IsFalse(tasks.Any(x => x.TGID == new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksSubTasks.Any(x => x.TSTGID == new Guid("f12acc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksSubTasks.Any(x => x.TSTGID == new Guid("f13acc1d-7bee-4635-9c4c-9404a4af80dd")));

            context.Verify(x => x.DeleteTask(It.IsAny<Cores.Entities.Tasks>()), Times.Once);
            context.Verify(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>()), Times.Never);
            context.Verify(x => x.DeleteTaskSubTask(It.IsAny<Cores.Entities.TasksSubTasks>()), Times.Exactly(2));
        }

        [Test]
        public void TestDeleteWithRelatedEntitiesCommandHandler_DeleteTask_TaskIsFound_DeleteTaskSubTasksAndTaskNotesAndTask_ShouldDeleteTask_AndRelatedTaskSubTasksAndTaskNotesAndTask()
        {
            //Arrange
            var model = new TasksDeleteTaskRelatedEntitiesViewModel()
            {
                TGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                DeleteTaskSubTasks = true,
                DeleteTaskNotes = true,
            };

            var command = new DeleteTaskRelatedEntitiesCommand() { Model = model };
            var handler = new DeleteTaskRelatedEntitiesCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(2, tasks.Count);
            ClassicAssert.AreEqual(1, tasksNotes.Count);
            ClassicAssert.AreEqual(1, tasksSubTasks.Count);

            ClassicAssert.IsFalse(tasks.Any(x => x.TGID == new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksNotes.Any(x => x.TNGID == new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksNotes.Any(x => x.TNGID == new Guid("f10acc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksSubTasks.Any(x => x.TSTGID == new Guid("f12acc1d-7bee-4635-9c4c-9404a4af80dd")));
            ClassicAssert.IsFalse(tasksSubTasks.Any(x => x.TSTGID == new Guid("f13acc1d-7bee-4635-9c4c-9404a4af80dd")));

            context.Verify(x => x.DeleteTask(It.IsAny<Cores.Entities.Tasks>()), Times.Once);
            context.Verify(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>()), Times.Exactly(2));
            context.Verify(x => x.DeleteTaskSubTask(It.IsAny<Cores.Entities.TasksSubTasks>()), Times.Exactly(2));
        }
    }
}
