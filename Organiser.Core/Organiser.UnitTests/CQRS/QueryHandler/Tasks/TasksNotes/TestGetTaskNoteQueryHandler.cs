using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
using Organiser.Core.Exceptions.Notes;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.QueryHandler.Tasks.TasksNotes
{
    [TestFixture]
    public class TestGetTaskNoteQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.Tasks>? tasks;
        private List<Cores.Entities.TasksNotes>? tasksNotes;

        public List<TasksNotesViewModel>? tasksNotesViewModel;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            mapper = new Mock<IMapper>();

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd")
                }
            };

            tasksNotes = new List<Cores.Entities.TasksNotes>()
            {
                new Cores.Entities.TasksNotes()
                {
                    TNID = 1,
                    TNGID = new Guid("98dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = tasks[0].TGID
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 2,
                    TNGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = tasks[0].TGID
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 3,
                    TNGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = tasks[0].TGID
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 4,
                    TNGID = new Guid("11dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNTGID = new Guid("67dacc1d-7bee-4635-9c4c-9404a4af80dd")
                },
            };

            tasksNotesViewModel = new List<TasksNotesViewModel>();

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.TasksNotes).Returns(tasksNotes.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.TasksNotes, TasksNotesViewModel>(It.IsAny<Cores.Entities.TasksNotes>()))
                .Callback((Cores.Entities.TasksNotes tasksNotes) =>
                    tasksNotesViewModel.Add(
                        new TasksNotesViewModel()
                        {
                            TNID = tasksNotes.TNID,
                            TNGID = tasksNotes.TNGID,
                        }
                    )
                );
        }

        [Test]
        public void TestGetTaskNoteQueryHandler_TaskNotFound_ShouldThrowNoteNotFoundException()
        {
            //Arrange
            var query = new GetTaskNoteQuery() { TGID = new Guid("69dacc1d-7bee-4635-9c4c-9404a4af80dd") };
            var handler = new GetTaskNoteQueryHandler(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<NoteNotFoundException>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetTaskNoteQueryHandler_TaskFound_Skip0_Take1_ShouldReturn_OneTaskNotesForTask()
        {
            //Arrange
            var query = new GetTaskNoteQuery() { TGID = tasks[0].TGID, Skip = 0, Take = 1 };
            var handler = new GetTaskNoteQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, tasksNotesViewModel.Count);
            ClassicAssert.AreEqual(3, result.Count);

            ClassicAssert.IsTrue(tasksNotes[0].TNGID == tasksNotesViewModel[0].TNGID);
        }

        [Test]
        public void TestGetTaskNoteQueryHandler_TaskFound_Skip1_Take1_ShouldReturn_OneTaskNotesForTask()
        {
            //Arrange
            var query = new GetTaskNoteQuery() { TGID = tasks[0].TGID, Skip = 1, Take = 1 };
            var handler = new GetTaskNoteQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, tasksNotesViewModel.Count);
            ClassicAssert.AreEqual(3, result.Count);

            ClassicAssert.IsTrue(tasksNotes[1].TNGID == tasksNotesViewModel[0].TNGID);
        }

        [Test]
        public void TestGetTaskNoteQueryHandler_TaskFound_Skip0_Take_10_ShouldReturn_ThreeTaskNotesForTask()
        {
            //Arrange
            var query = new GetTaskNoteQuery() { TGID = tasks[0].TGID, Skip = 0, Take = 10 };
            var handler = new GetTaskNoteQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, tasksNotesViewModel.Count);
            ClassicAssert.AreEqual(3, result.Count);

            ClassicAssert.IsTrue(tasksNotes[0].TNGID == tasksNotesViewModel[0].TNGID);
            ClassicAssert.IsTrue(tasksNotes[1].TNGID == tasksNotesViewModel[1].TNGID);
            ClassicAssert.IsTrue(tasksNotes[2].TNGID == tasksNotesViewModel[2].TNGID);
        }
    }
}
