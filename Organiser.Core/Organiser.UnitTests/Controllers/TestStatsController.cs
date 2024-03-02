//using Moq;
//using NUnit.Framework;
//using NUnit.Framework.Legacy;
//using Organiser.Cores.Context;
//using Organiser.Cores.Controllers;
//using Organiser.Cores.Entities;
//using Organiser.Cores.Models.Enums;

//namespace Organiser.UnitTests.Controllers
//{
//    [TestFixture]
//    public class TestStatsController
//    {
//        private Mock<IDataBaseContext>? context;

//        private List<Savings>? savings;
//        private List<Tasks>? tasks;
//        private List<Categories>? categories;

//        [SetUp]
//        public void SetUp()
//        {
//            context = new Mock<IDataBaseContext>();

//            savings = new List<Savings>()
//            {
//                new Savings()
//                {
//                    SID = 1,
//                    SGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    SUID = 1,
//                    SAmount = 245.5M,
//                    SOnWhat = "OnWhat1",
//                    STime = new DateTime(2024, 1, 5, 23, 55, 1),
//                    SWhere = "Where1",
//                },
//                new Savings()
//                {
//                    SID = 2,
//                    SGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    SUID = 1,
//                    SAmount = 2135.9M,
//                    SOnWhat = "OnWhat2",
//                    STime = new DateTime(2024, 1, 30, 23, 55, 1),
//                    SWhere = "Where2",
//                },
//                new Savings()
//                {
//                    SID = 3,
//                    SGID = new Guid("20dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    SUID = 1,
//                    SAmount = 4455.1M,
//                    SOnWhat = "OnWhat3",
//                    STime = new DateTime(2023, 12, 15, 23, 55, 1),
//                    SWhere = "Where3",
//                },
//            };

//            tasks = new List<Tasks>()
//            {
//                new Tasks()
//                {
//                    TID = 1,
//                    TGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 1,
//                    TBudget = 245,
//                    TName = "Name1",
//                    TTime = new DateTime(2024, 1, 5, 23, 55, 1),
//                    TLocalization = "Localization1",
//                    TStatus = TaskEnum.NotStarted,
//                },
//                new Tasks()
//                {
//                    TID = 2,
//                    TGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 1,
//                    TBudget = 2135,
//                    TName = "Name2",
//                    TTime = new DateTime(2024, 1, 30, 23, 55, 1),
//                    TLocalization = "Localization2",
//                    TStatus = TaskEnum.OnGoing,
//                },
//                new Tasks()
//                {
//                    TID = 3,
//                    TGID = new Guid("20dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TCGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    TUID = 1,
//                    TBudget = 4455,
//                    TName = "Name3",
//                    TTime = new DateTime(2023, 12, 15, 23, 55, 1),
//                    TLocalization = "Localization3",
//                    TStatus = TaskEnum.OnGoing,
//                },
//            };

//            categories = new List<Categories>()
//            {
//                new Categories()
//                {
//                    CID = 1,
//                    CGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    CUID = 1,
//                    CName = "Nazwa 1",
//                    CBudget = 24555,
//                    CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
//                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
//                },
//            };

//            context.Setup(x => x.Savings).Returns(savings.AsQueryable());
//            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
//            context.Setup(x => x.Categories).Returns(categories.AsQueryable());
//        }

//        //GetSavingBarChart
//        [Test]
//        public void TestStatsController_GetSavingBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetSavingBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

//            //Assert
//            ClassicAssert.AreEqual(1, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

//            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
//            ClassicAssert.AreEqual(2381.4M, result?.Datasets?.Data[0]);
//        }

//        [Test]
//        public void TestStatsController_GetSavingBarChart_TakeSavingsOnlyFromFirstHalfJanuary2024_ShouldReturnDataFromSavingsOnlyFromFirstHalfOfJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetSavingBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 14, 23, 59, 59));

//            //Assert
//            ClassicAssert.AreEqual(1, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

//            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
//            ClassicAssert.AreEqual(245.5M, result?.Datasets?.Data[0]);
//        }

//        [Test]
//        public void TestStatsController_GetSavingBarChart_TakeSavingsFromDecember2023AndJanuary2024_ShouldReturnDataFromSavingsFromFDecember2023AndJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetSavingBarChart(new DateTime(2023, 12, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

//            //Assert
//            ClassicAssert.AreEqual(2, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

//            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
//            ClassicAssert.AreEqual(4455.1M, result?.Datasets?.Data[0]);
//            ClassicAssert.AreEqual(2381.4M, result?.Datasets?.Data[1]);
//        }

//        //GetMoneySpendedFromTaskBarChart
//        [Test]
//        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedFromTaskBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

//            //Assert
//            ClassicAssert.AreEqual(1, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

//            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
//            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[0]);
//        }

//        [Test]
//        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsOnlyFromFirstHalfJanuary2024_ShouldReturnDataFromSavingsOnlyFromFirstHalfOfJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedFromTaskBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 14, 23, 59, 59));

//            //Assert
//            ClassicAssert.AreEqual(1, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

//            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
//            ClassicAssert.AreEqual(245M, result?.Datasets?.Data[0]);
//        }

//        [Test]
//        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsFromDecember2023AndJanuary2024_ShouldReturnDataFromSavingsFromFDecember2023AndJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedFromTaskBarChart(new DateTime(2023, 12, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

//            //Assert
//            ClassicAssert.AreEqual(2, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

//            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
//            ClassicAssert.AreEqual(4455M, result?.Datasets?.Data[0]);
//            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[1]);
//        }

//        //GetMoneySpendedForCategoryBarChart
//        [Test]
//        public void TestStatsController_GetMoneySpendedForCategoryBarChart_CGIDIsEmpty_ShouldReturnEmptyDataSets()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedForCategoryBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59), Guid.Empty);

//            //Assert
//            ClassicAssert.AreEqual(null, result?.Labels?.Count);
//            ClassicAssert.AreEqual(null, result?.Datasets?.Data?.Count);
//        }

//        [Test]
//        public void TestStatsController_GetMoneySpendedForCategoryBarChart_CGIDIsNotFound_ShouldThrowException()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            //Assert
//            Assert.Throws<Exception>(() => controller.GetMoneySpendedForCategoryBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59), Guid.NewGuid()));
//        }

//        [Test]
//        public void TestStatsController_GetMoneySpendedForCategoryBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedForCategoryBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59), categories[0].CGID);

//            //Assert
//            ClassicAssert.AreEqual(1, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

//            ClassicAssert.AreEqual("Wydane pieniądze na daną kategorię", result?.Datasets.Label);
//            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[0]);
//        }

//        [Test]
//        public void TestStatsController_GetMoneySpendedForCategoryBarChart_TakeSavingsOnlyFromFirstHalfJanuary2024_ShouldReturnDataFromSavingsOnlyFromFirstHalfOfJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedForCategoryBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 14, 23, 59, 59), categories[0].CGID);

//            //Assert
//            ClassicAssert.AreEqual(1, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[0]);

//            ClassicAssert.AreEqual("Wydane pieniądze na daną kategorię", result?.Datasets.Label);
//            ClassicAssert.AreEqual(245M, result?.Datasets?.Data[0]);
//        }

//        [Test]
//        public void TestStatsController_GetMoneySpendedForCategoryBarChart_TakeSavingsFromDecember2023AndJanuary2024_ShouldReturnDataFromSavingsFromFDecember2023AndJanuary2024()
//        {
//            //Arrange
//            var controller = new StatsController(context.Object);

//            //Act
//            var result = controller.GetMoneySpendedForCategoryBarChart(new DateTime(2023, 12, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59), categories[0].CGID);

//            //Assert
//            ClassicAssert.AreEqual(2, result?.Labels?.Count);
//            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
//            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

//            ClassicAssert.AreEqual("Wydane pieniądze na daną kategorię", result?.Datasets.Label);
//            ClassicAssert.AreEqual(4455M, result?.Datasets?.Data[0]);
//            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[1]);
//        }
//    }
//}