using AutoMapper;
using Moq;
using NUnit.Framework;
using Organiser.Cores;
using Organiser.Cores.Controllers;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestTasksController
    {
        private Mock<DataContext>? context;
        private Mock<IMapper>? mapper;

        private List<Tasks>? tasks;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<DataContext>();
            mapper = new Mock<IMapper>();

            tasks = new List<Tasks>()
            {
                new Tasks()
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
                new Tasks()
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
                new Tasks()
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
                new Tasks()
                {
                    TID = 4,
                    TGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f0dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 44,
                    TLocalization = "Lokalizacja 4",
                    TName = "Nazwa 4",
                    TBudget = 2050,
                    TStatus = TaskEnum.Done,
                    TTime = new DateTime(2023, 12, 4, 21, 30, 0)
                },
            };

            context.Setup(x => x.Add(It.IsAny<Tasks>())).Callback<Tasks>(tasks.Add);
        }

        [Test]
        public void TestTasksController_GetAllDataForUser_ShouldReturn3Tasks()
        {
            //Arrange
            var controller = new TasksController(context.Object, mapper.Object);

            //Act
            var result = controller.Get();

            //Assert
            NUnit.Framework.Assert.Equals(3, result.Count);
            NUnit.Framework.Assert.That(result.All(x => x.TUID == 1));

        }
    }
}
