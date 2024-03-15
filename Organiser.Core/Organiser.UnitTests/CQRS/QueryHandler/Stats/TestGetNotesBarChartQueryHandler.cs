using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Stats.Handlers;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.QueryHandler.Stats
{
    [TestFixture]
    public class TestGetNotesBarChartQueryHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Notes>? notes;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            notes = new List<Cores.Entities.Notes>()
            {
                new Cores.Entities.Notes()
                {
                    NID = 1,
                    NGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NUID = 1,
                    NDate = new DateTime(2024, 1, 5, 23, 55, 1),
                },
                new Cores.Entities.Notes()
                {
                    NID = 2,
                    NGID = new Guid("01dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NUID = 1,
                    NDate = new DateTime(2024, 1, 30, 23, 55, 1),
                },
                new Cores.Entities.Notes()
                {
                    NID = 3,
                    NGID = new Guid("02dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NUID = 1,
                    NDate = new DateTime(2023, 12, 15, 23, 55, 1),
                },
            };

            context.Setup(x => x.Notes).Returns(notes.AsQueryable());
        }

        [Test]
        public void TestGetNotesBarChartQueryHandler_TakeNotesOnlyFromJanuary2024_ShouldReturnDataFromNotesFromJanuary2024()
        {
            //Arrange
            var query = new GetNotesBarChartQuery() { StartDate = new DateTime(2024, 1, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 31, 23, 59, 59) };
            var handler = new GetNotesBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result?.Labels?.Count);
            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

            ClassicAssert.AreEqual("Liczba notatek", result?.Datasets.Label);
            ClassicAssert.AreEqual(2M, result?.Datasets?.Data[0]);
        }

        [Test]
        public void TestGetNotesBarChartQueryHandler_TakeNotesOnlyFromFirstHalfJanuary2024_ShouldReturnDataFromNotesOnlyFromFirstHalfOfJanuary2024()
        {
            //Arrange
            var query = new GetNotesBarChartQuery() { StartDate = new DateTime(2024, 1, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 14, 23, 59, 59) };
            var handler = new GetNotesBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result?.Labels?.Count);
            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

            ClassicAssert.AreEqual("Liczba notatek", result?.Datasets.Label);
            ClassicAssert.AreEqual(1, result?.Datasets?.Data[0]);
        }

        [Test]
        public void TestGetNotesBarChartQueryHandler_TakeNotesFromDecember2023AndJanuary2024_ShouldReturnDataFromNotesFromDecember2023AndJanuary2024()
        {
            //Arrange
            var query = new GetNotesBarChartQuery() { StartDate = new DateTime(2023, 12, 1, 0, 0, 0), EndDate = new DateTime(2024, 1, 31, 23, 59, 59) };
            var handler = new GetNotesBarChartQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result?.Labels?.Count);
            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

            ClassicAssert.AreEqual("Liczba notatek", result?.Datasets.Label);
            ClassicAssert.AreEqual(1M, result?.Datasets?.Data[0]);
            ClassicAssert.AreEqual(2M, result?.Datasets?.Data[1]);
        }
    }
}
