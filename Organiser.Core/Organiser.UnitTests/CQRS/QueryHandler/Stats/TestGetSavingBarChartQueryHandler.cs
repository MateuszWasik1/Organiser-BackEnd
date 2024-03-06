using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Stats.Handlers;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.QueryHandler.Stats
{
    [TestFixture]
    public class TestGetSavingBarChartQueryHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Savings>? savings;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            savings = new List<Cores.Entities.Savings>()
            {
                new Cores.Entities.Savings()
                {
                    SID = 1,
                    SGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 245.5M,
                    SOnWhat = "OnWhat1",
                    STime = new DateTime(2024, 1, 5, 23, 55, 1),
                    SWhere = "Where1",
                },
                new Cores.Entities.Savings()
                {
                    SID = 2,
                    SGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 2135.9M,
                    SOnWhat = "OnWhat2",
                    STime = new DateTime(2024, 1, 30, 23, 55, 1),
                    SWhere = "Where2",
                },
                new Cores.Entities.Savings()
                {
                    SID = 3,
                    SGID = new Guid("20dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 4455.1M,
                    SOnWhat = "OnWhat3",
                    STime = new DateTime(2023, 12, 15, 23, 55, 1),
                    SWhere = "Where3",
                },
            };

            context.Setup(x => x.Savings).Returns(savings.AsQueryable());
        }

        //GetSavingBarChart
        [Test]
        public void TestStatsController_GetSavingBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
        {
            //Arrange
            var query = new GetSavingBarChartQuery() { StartDate = new DateTime(2024, 1, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 31, 23, 59, 59) };
            var handler = new GetSavingBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result?.Labels?.Count);
            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
            ClassicAssert.AreEqual(2381.4M, result?.Datasets?.Data[0]);
        }

        [Test]
        public void TestStatsController_GetSavingBarChart_TakeSavingsOnlyFromFirstHalfJanuary2024_ShouldReturnDataFromSavingsOnlyFromFirstHalfOfJanuary2024()
        {
            //Arrange
            var query = new GetSavingBarChartQuery() { StartDate = new DateTime(2024, 1, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 14, 23, 59, 59) };
            var handler = new GetSavingBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result?.Labels?.Count);
            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
            ClassicAssert.AreEqual(245.5M, result?.Datasets?.Data[0]);
        }

        [Test]
        public void TestStatsController_GetSavingBarChart_TakeSavingsFromDecember2023AndJanuary2024_ShouldReturnDataFromSavingsFromFDecember2023AndJanuary2024()
        {
            //Arrange
            var query = new GetSavingBarChartQuery() { StartDate = new DateTime(2023, 12, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 31, 23, 59, 59) };
            var handler = new GetSavingBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result?.Labels?.Count);
            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
            ClassicAssert.AreEqual(4455.1M, result?.Datasets?.Data[0]);
            ClassicAssert.AreEqual(2381.4M, result?.Datasets?.Data[1]);
        }
    }
}
