using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Stats.Handlers;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.QueryHandler.Stats
{
    [TestFixture]
    public class TestGetMoneySpendedFromTaskBarChartQueryHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Tasks>? tasks;
        private List<Cores.Entities.Categories>? categories;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TBudget = 245,
                    TName = "Name1",
                    TTime = new DateTime(2024, 1, 5, 23, 55, 1),
                    TLocalization = "Localization1",
                    TStatus = TaskEnum.NotStarted,
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TBudget = 2135,
                    TName = "Name2",
                    TTime = new DateTime(2024, 1, 30, 23, 55, 1),
                    TLocalization = "Localization2",
                    TStatus = TaskEnum.OnGoing,
                },
                new Cores.Entities.Tasks()
                {
                    TID = 3,
                    TGID = new Guid("20dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TBudget = 4455,
                    TName = "Name3",
                    TTime = new DateTime(2023, 12, 15, 23, 55, 1),
                    TLocalization = "Localization3",
                    TStatus = TaskEnum.OnGoing,
                },
            };

            categories = new List<Cores.Entities.Categories>()
            {
                new Cores.Entities.Categories()
                {
                    CID = 1,
                    CGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 1",
                    CBudget = 24555,
                    CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
                },
            };

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.Categories).Returns(categories.AsQueryable());
        }

        [Test]
        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
        {
            //Arrange
            var query = new GetMoneySpendedFromTaskBarChartQuery() { StartDate = new DateTime(2024, 1, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 31, 23, 59, 59) };
            var handler = new GetMoneySpendedFromTaskBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result?.Labels?.Count);
            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[0]);
        }

        [Test]
        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsOnlyFromFirstHalfJanuary2024_ShouldReturnDataFromSavingsOnlyFromFirstHalfOfJanuary2024()
        {
            //Arrange
            var query = new GetMoneySpendedFromTaskBarChartQuery() { StartDate = new DateTime(2024, 1, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 14, 23, 59, 59) };
            var handler = new GetMoneySpendedFromTaskBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result?.Labels?.Count);
            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
            ClassicAssert.AreEqual(245M, result?.Datasets?.Data[0]);
        }

        [Test]
        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsFromDecember2023AndJanuary2024_ShouldReturnDataFromSavingsFromFDecember2023AndJanuary2024()
        {
            //Arrange
            var query = new GetMoneySpendedFromTaskBarChartQuery() { StartDate = new DateTime(2023, 12, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 31, 23, 59, 59) };
            var handler = new GetMoneySpendedFromTaskBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result?.Labels?.Count);
            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
            ClassicAssert.AreEqual(4455M, result?.Datasets?.Data[0]);
            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[1]);
        }
    }
}
