//using AutoMapper;
//using Moq;
//using NUnit.Framework;
//using NUnit.Framework.Legacy;
//using Organiser.Cores.Context;
//using Organiser.Cores.Controllers;
//using Organiser.Cores.Entities;
//using Organiser.Cores.Models.Enums;
//using Organiser.Cores.Models.ViewModels;
//using Organiser.Cores.Services;

//namespace Organiser.UnitTests.Controllers
//{
//    [TestFixture]
//    public class TestTasksController
//    {
//        private Mock<IDataBaseContext>? context;
//        private Mock<IUserContext>? user;
//        private Mock<IMapper>? mapper;

//        private List<Tasks>? tasks;

//        [SetUp]
//        public void SetUp()
//        {
//            context = new Mock<IDataBaseContext>();
//            user = new Mock<IUserContext>();
//            mapper = new Mock<IMapper>();

//            tasks = new List<Tasks>()
//            {
//                new Tasks()
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
//                new Tasks()
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
//                new Tasks()
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
//                new Tasks()
//                {
//                    TID = 4,
//                    TGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f0dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 44,
//                    TLocalization = "Lokalizacja 4",
//                    TName = "Nazwa 4",
//                    TBudget = 2050,
//                    TStatus = TaskEnum.Done,
//                    TTime = new DateTime(2023, 12, 4, 21, 30, 0)
//                },
//            };

//            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());

//            context.Setup(x => x.CreateOrUpdate(It.IsAny<Tasks>())).Callback<Tasks>(task => 
//            { 
//                var currentTask = tasks.FirstOrDefault(x => x.TID == task.TID);

//                if (currentTask != null)
//                    tasks[tasks.FindIndex(x => x.TID == currentTask.TID)] = task;
//                else
//                    tasks.Add(task);
//            });

//            context.Setup(x => x.DeleteTask(It.IsAny<Tasks>())).Callback<Tasks>(task => tasks.Remove(task));

//            mapper.Setup(x => x.Map(It.IsAny<Tasks>(), It.IsAny<TasksViewModel>())).Returns<Tasks, TasksViewModel>((task, model) =>
//            {
//                return new TasksViewModel()
//                {
//                    TID = task.TID,
//                    TGID = task.TGID,
//                    TCGID = task.TGID,
//                    TUID = task.TUID,
//                    TLocalization = task.TLocalization,
//                    TName = task.TName,
//                    TBudget = task.TBudget,
//                    TStatus = task.TStatus,
//                    TTime = task.TTime,
//                };
//            });
//        }

//        //Get
//        [Test]
//        public void TestTasksController_GetAllDataForUser_ShouldReturn4Tasks()
//        {
//            //Arrange
//            var controller = new TasksController(context.Object, user.Object, mapper.Object);

//            //Act
//            var result = controller.Get();

//            //Assert
//            ClassicAssert.AreEqual(4, result.Count);
//        }

//        //Post
//        [Test]
//        public void TestTasksController_AddTask_ShouldAddTask()
//        {
//            //Arrange
//            var controller = new TasksController(context.Object, user.Object, mapper.Object);

//            var task = new TasksViewModel()
//            {
//                TID = 0,
//                TGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                TCGID = new Guid("f0dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                TUID = 44,
//                TLocalization = "Lokalizacja 5",
//                TName = "Nazwa 5",
//                TBudget = 2050,
//                TStatus = TaskEnum.Done,
//                TTime = new DateTime(2023, 12, 4, 21, 30, 0)
//            };

//            //Act
//            controller.Save(task);

//            //Assert
//            ClassicAssert.AreEqual(5, tasks.Count);
//            ClassicAssert.AreEqual("Nazwa 5", tasks[4].TName);
//            ClassicAssert.AreEqual("Lokalizacja 5", tasks[4].TLocalization);
//            ClassicAssert.AreEqual(2050, tasks[4].TBudget);
//            ClassicAssert.AreEqual(TaskEnum.Done, tasks[4].TStatus);
//            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), tasks[4].TTime);
//        }

//        [Test]
//        public void TestTasksController_AddTask_TaskExistButErrorIsThrow_ShouldThrowException()
//        {
//            //Arrange
//            var controller = new TasksController(context.Object, user.Object, mapper.Object);

//            var task = new TasksViewModel()
//            {
//                TID = 2,
//                TGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//            };

//            //Act
//            //Assert
//            Assert.Throws<Exception>(() => controller.Save(task));
//        }

//        [Test]
//        public void TestTasksController_AddTask_TaskExist_ShouldModifyTask()
//        {
//            //Arrange
//            var controller = new TasksController(context.Object, user.Object, mapper.Object);

//            var task = new TasksViewModel()
//            {
//                TID = 4,
//                TGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                TUID = 44,
//                TLocalization = "Lokalizacja 5",
//                TName = "Nazwa 5",
//                TBudget = 2060,
//                TStatus = TaskEnum.OnGoing,
//                TTime = new DateTime(2023, 12, 5, 21, 30, 0)
//            };

//            //Act
//            controller.Save(task);

//            //Assert
//            ClassicAssert.AreEqual(4, tasks.Count);
//            ClassicAssert.AreEqual("Nazwa 5", tasks[3].TName);
//            ClassicAssert.AreEqual("Lokalizacja 5", tasks[3].TLocalization);
//            ClassicAssert.AreEqual(2060, tasks[3].TBudget);
//            ClassicAssert.AreEqual(TaskEnum.OnGoing, tasks[3].TStatus);
//            ClassicAssert.AreEqual(new DateTime(2023, 12, 5, 21, 30, 0), tasks[3].TTime);
//        }

//        //Delete
//        [Test]
//        public void TestTasksController_DeleteTask_TaskNotFound_ShouldThrowException()
//        {
//            //Arrange
//            var controller = new TasksController(context.Object, user.Object, mapper.Object);

//            //Act
//            //Assert
//            Assert.Throws<Exception>(() => controller.Delete(Guid.Empty));
//        }

//        [Test]
//        public void TestTasksController_DeleteTask_TaskIsFound_ShouldDeleteTask()
//        {
//            //Arrange
//            var controller = new TasksController(context.Object, user.Object, mapper.Object);

//            //Act
//            controller.Delete(tasks[2].TGID);

//            //Assert
//            ClassicAssert.AreEqual(3, tasks.Count);
//        }
//    }
//}