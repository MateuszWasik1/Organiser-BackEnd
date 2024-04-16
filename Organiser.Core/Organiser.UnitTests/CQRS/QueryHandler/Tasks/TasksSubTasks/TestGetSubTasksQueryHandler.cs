using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Queries;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.QueryHandler.Tasks.TasksSubTasks
{
    [TestFixture]
    public class TestGetSubTasksQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IMapper> mapper;

        public List<Cores.Entities.Tasks> tasks;
        public List<Cores.Entities.TasksSubTasks> subTasks;
        private List<TasksSubTasksViewModel> tasksSubTasksViewModel;

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
                    TGID = new Guid("28dd879c-ee2f-11db-8314-0800200c9a66"),
                    TUID = 1,
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TGID = new Guid("29dd879c-ee2f-11db-8314-0800200c9a66"),
                    TUID = 1,
                },
            };

            subTasks = new List<Cores.Entities.TasksSubTasks>()
            {
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 1,
                    TSTGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTTGID = tasks[0].TGID,
                    TSTUID = 1,
                    TSTStatus = SubTasksStatusEnum.NotStarted,
                },
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 2,
                    TSTGID = new Guid("31dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTTGID = tasks[0].TGID,
                    TSTUID = 1,
                    TSTStatus = SubTasksStatusEnum.Done,
                },
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 3,
                    TSTGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTTGID = tasks[0].TGID,
                    TSTUID = 1,
                    TSTStatus = SubTasksStatusEnum.Done,
                },
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 4,
                    TSTGID = new Guid("34dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTTGID = new Guid("35dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTUID = 1,
                    TSTStatus = SubTasksStatusEnum.Done,
                },
            };

            tasksSubTasksViewModel = new List<TasksSubTasksViewModel>();

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.TasksSubTasks).Returns(subTasks.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.TasksSubTasks, TasksSubTasksViewModel>(It.IsAny<Cores.Entities.TasksSubTasks>()))
                .Callback((Cores.Entities.TasksSubTasks subTask) =>
                    tasksSubTasksViewModel.Add(
                        new TasksSubTasksViewModel()
                        {
                            TSTGID = subTask.TSTGID,
                            TSTStatus = subTask.TSTStatus,
                        }
                    )
                );
        }

        [Test]
        public void TestGetSubTasksQueryHandler_TaskIsNotFound_ShouldThrowTaskNotFoundException()
        {
            //Arrange 
            var query = new GetSubTasksQuery() { TGID = Guid.NewGuid() };
            var handler = new GetSubTasksQueryHandler(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotFoundException>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetSubTasksQueryHandler_GetSubTaskForTask_Skip0_Take1_ShouldReturn_OneTasksSubTasks()
        {
            //Arrange
            var query = new GetSubTasksQuery() { TGID = tasks[0].TGID, Skip = 0, Take = 1 };
            var handler = new GetSubTasksQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, tasksSubTasksViewModel.Count);
            ClassicAssert.AreEqual(3, result.Count);

            ClassicAssert.AreEqual(subTasks[0].TSTGID, tasksSubTasksViewModel[0].TSTGID);
            ClassicAssert.AreEqual(subTasks[0].TSTStatus, tasksSubTasksViewModel[0].TSTStatus);
        }

        [Test]
        public void TestGetSubTasksQueryHandler_GetSubTaskForTask_Skip1_Take1_ShouldReturn_OneTasksSubTasks()
        {
            //Arrange
            var query = new GetSubTasksQuery() { TGID = tasks[0].TGID, Skip = 1, Take = 1 };
            var handler = new GetSubTasksQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, tasksSubTasksViewModel.Count);
            ClassicAssert.AreEqual(3, result.Count);

            ClassicAssert.AreEqual(subTasks[1].TSTGID, tasksSubTasksViewModel[0].TSTGID);
            ClassicAssert.AreEqual(subTasks[1].TSTStatus, tasksSubTasksViewModel[0].TSTStatus);
        }

        [Test]
        public void TestGetSubTasksQueryHandler_GetSubTaskForTask_Skip0_Take10_ShouldReturn_ThreeTasksSubTasks()
        {
            //Arrange
            var query = new GetSubTasksQuery() { TGID = tasks[0].TGID, Skip = 0, Take = 10 };
            var handler = new GetSubTasksQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, tasksSubTasksViewModel.Count);
            ClassicAssert.AreEqual(3, result.Count);

            ClassicAssert.AreEqual(subTasks[0].TSTGID, tasksSubTasksViewModel[0].TSTGID);
            ClassicAssert.AreEqual(subTasks[0].TSTStatus, tasksSubTasksViewModel[0].TSTStatus);

            ClassicAssert.AreEqual(subTasks[1].TSTGID, tasksSubTasksViewModel[1].TSTGID);
            ClassicAssert.AreEqual(subTasks[1].TSTStatus, tasksSubTasksViewModel[1].TSTStatus);

            ClassicAssert.AreEqual(subTasks[2].TSTGID, tasksSubTasksViewModel[2].TSTGID);
            ClassicAssert.AreEqual(subTasks[2].TSTStatus, tasksSubTasksViewModel[2].TSTStatus);
        }
    }
}
