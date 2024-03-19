using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Queries;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.QueryHandler.Notes
{
    [TestFixture]
    public class TestGetTaskQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.Tasks>? tasks;

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
                    TUID = 1,
                    TGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f0dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TLocalization = "Where1",
                    TTime = new DateTime(2024, 3, 18, 18, 36, 0),
                    TName = "OnWhat1",
                    TBudget = 234,
                    TStatus = TaskEnum.Done
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TUID = 1,
                    TGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f1dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TLocalization = "Where2",
                    TTime = new DateTime(2024, 3, 18, 18, 36, 0),
                    TName = "Name2",
                    TBudget = 2334,
                    TStatus = TaskEnum.NotStarted
                },
            };

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Tasks, TaskViewModel>(It.IsAny<Cores.Entities.Tasks>()))
                .Returns((Cores.Entities.Tasks task) =>
                    new TaskViewModel()
                    {
                        TGID = task.TGID,
                        TCGID = task.TCGID,
                        TBudget = task.TBudget,
                        TLocalization = task.TLocalization,
                        TName = task.TName,
                        TStatus = task.TStatus,
                        TTime = task.TTime,
                    }
                );
        }

        [Test]
        public void TestGetTaskQueryHandler_TaskNotFound_ShouldThrowException()
        {
            //Arrange
            var query = new GetTaskQuery() { TGID = new Guid() };
            var handler = new GetTaskQueryHandler(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetTaskQueryHandler_TaskWasFound_ShouldReturnTask()
        {
            //Arrange
            var query = new GetTaskQuery() { TGID = tasks[0].TGID };
            var handler = new GetTaskQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(tasks[0].TGID, result.TGID);
            ClassicAssert.AreEqual(tasks[0].TCGID, result.TCGID);
            ClassicAssert.AreEqual(tasks[0].TBudget, result.TBudget);
            ClassicAssert.AreEqual(tasks[0].TLocalization, result.TLocalization);
            ClassicAssert.AreEqual(tasks[0].TName, result.TName);
            ClassicAssert.AreEqual(tasks[0].TStatus, result.TStatus);
            ClassicAssert.AreEqual(tasks[0].TTime, result.TTime);
        }
    }
}