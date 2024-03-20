using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksNotes.Queries;
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
                    TNID = 2,
                    TNGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
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
        public void TestGetTaskNoteQueryHandler_TaskNotFound_ShouldThrowException()
        {
            //Arrange
            var query = new GetTaskNoteQuery() { TGID = new Guid("69dacc1d-7bee-4635-9c4c-9404a4af80dd") };
            var handler = new GetTaskNoteQueryHandler(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetTaskNoteQueryHandler_TaskFound_ShouldReturnTaskNotesForTask()
        {
            //Arrange
            var query = new GetTaskNoteQuery() { TGID = tasks[0].TGID };
            var handler = new GetTaskNoteQueryHandler(context.Object, mapper.Object);

            //Act
            handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, tasksNotesViewModel.Count);

            ClassicAssert.IsTrue(tasksNotes[0].TNGID == tasksNotesViewModel[0].TNGID);
            ClassicAssert.IsTrue(tasksNotes[1].TNGID == tasksNotesViewModel[1].TNGID);
        }
    }
}
