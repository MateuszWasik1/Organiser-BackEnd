//using AutoMapper;
//using Moq;
//using NUnit.Framework;
//using NUnit.Framework.Legacy;
//using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
//using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
//using Organiser.Cores.Context;
//using Organiser.Cores.Models.Enums;
//using Organiser.Cores.Models.ViewModels;
//using Organiser.Cores.Services;

//namespace Organiser.UnitTests.CQRS.QueryHandler.Tasks.Tasks
//{
//    [TestFixture]
//    public class TestGetTasksQueryHandler
//    {
//        private Mock<IDataBaseContext>? context;
//        private Mock<IUserContext>? user;
//        private Mock<IMapper>? mapper;

//        public List<Cores.Entities.Tasks>? tasks;
//        public List<TasksViewModel> tasksViewModel;

//        [SetUp]
//        public void SetUp()
//        {
//            context = new Mock<IDataBaseContext>();
//            user = new Mock<IUserContext>();
//            mapper = new Mock<IMapper>();

//            tasks = new List<Cores.Entities.Tasks>()
//            {
//                new Cores.Entities.Tasks()
//                {
//                    TID = 1,
//                    TGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 1,
//                    TLocalization = "Lokalizacja 1",
//                    TName = "Nazwa 1",
//                    TBudget = 2050,
//                    TStatus = TaskEnum.NotStarted,
//                    TTime = new DateTime(2023, 12, 4, 21, 30, 0)
//                },
//                new Cores.Entities.Tasks()
//                {
//                    TID = 2,
//                    TGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f6dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 1,
//                    TLocalization = "Lokalizacja 2",
//                    TName = "Nazwa 2",
//                    TBudget = 2060,
//                    TStatus = TaskEnum.OnGoing,
//                    TTime = new DateTime(2023, 12, 5, 21, 30, 0)
//                },
//                new Cores.Entities.Tasks()
//                {
//                    TID = 3,
//                    TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 1,
//                    TLocalization = "Lokalizacja 3",
//                    TName = "Nazwa 3",
//                    TBudget = 2070,
//                    TStatus = TaskEnum.Done,
//                    TTime = new DateTime(2023, 12, 6, 21, 30, 0)
//                },
//            };

//            tasksViewModel = new List<TasksViewModel>();

//            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());

//            mapper.Setup(m => m.Map<Cores.Entities.Tasks, TasksViewModel>(It.IsAny<Cores.Entities.Tasks>()))
//                .Callback((Cores.Entities.Tasks task) =>
//                    tasksViewModel.Add(
//                        new TasksViewModel()
//                        {
//                            TID = task.TID,
//                            TGID = task.TGID,
//                            TBudget = task.TBudget,
//                            TCGID = task.TCGID,
//                            TLocalization = task.TLocalization,
//                            TName = task.TName,
//                            TStatus = task.TStatus,
//                            TTime = task.TTime,
//                            TUID = task.TUID,
//                        }
//                    )
//                );
//        }

//        [Test]
//        public void TestTasksController_GetAllDataForUser_ShouldReturn3Tasks()
//        {
//            //Arrange
//            var query = new GetTasksQuery() { CGID = Guid.Empty.ToString(), Status = 4 };
//            var handler = new GetTasksQueryHandler(context.Object, user.Object, mapper.Object);

//            //Act
//            handler.Handle(query);

//            //Assert
//            ClassicAssert.AreEqual(3, tasksViewModel.Count());
//        }

//        [Test]
//        public void TestTasksController_GetAllDataForUser_ShouldReturn1Tasks_WithStatus_NotStarted()
//        {
//            //Arrange
//            var query = new GetTasksQuery() { CGID = Guid.Empty.ToString(), Status = 1 };
//            var handler = new GetTasksQueryHandler(context.Object, user.Object, mapper.Object);

//            //Act
//            handler.Handle(query);

//            //Assert
//            ClassicAssert.AreEqual(1, tasksViewModel.Count());
//            ClassicAssert.IsTrue(tasksViewModel.Any(x => x.TStatus == TaskEnum.NotStarted));
//            ClassicAssert.IsFalse(tasksViewModel.Any(x => x.TStatus == TaskEnum.OnGoing));
//            ClassicAssert.IsFalse(tasksViewModel.Any(x => x.TStatus == TaskEnum.Done));
//        }

//        [Test]
//        public void TestTasksController_GetAllDataForUser_ShouldReturn1Tasks_WithStatus_OnGoing()
//        {
//            //Arrange
//            var query = new GetTasksQuery() { CGID = Guid.Empty.ToString(), Status = 2 };
//            var handler = new GetTasksQueryHandler(context.Object, user.Object, mapper.Object);

//            //Act
//            handler.Handle(query);

//            //Assert
//            ClassicAssert.AreEqual(1, tasksViewModel.Count());
//            ClassicAssert.IsFalse(tasksViewModel.Any(x => x.TStatus == TaskEnum.NotStarted));
//            ClassicAssert.IsTrue(tasksViewModel.Any(x => x.TStatus == TaskEnum.OnGoing));
//            ClassicAssert.IsFalse(tasksViewModel.Any(x => x.TStatus == TaskEnum.Done));
//        }

//        [Test]
//        public void TestTasksController_GetAllDataForUser_ShouldReturn1Tasks_WithStatus_Done()
//        {
//            //Arrange
//            var query = new GetTasksQuery() { CGID = Guid.Empty.ToString(), Status = 3 };
//            var handler = new GetTasksQueryHandler(context.Object, user.Object, mapper.Object);

//            //Act
//            handler.Handle(query);

//            //Assert
//            ClassicAssert.AreEqual(1, tasksViewModel.Count());
//            ClassicAssert.IsFalse(tasksViewModel.Any(x => x.TStatus == TaskEnum.NotStarted));
//            ClassicAssert.IsFalse(tasksViewModel.Any(x => x.TStatus == TaskEnum.OnGoing));
//            ClassicAssert.IsTrue(tasksViewModel.Any(x => x.TStatus == TaskEnum.Done));
//        }
//    }
//}
