using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Cores.Context;
using Organiser.Cores.Controllers;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestStatsController
    {
        private Mock<IDataBaseContext>? context;

        private List<Savings>? savings;
        private List<Tasks>? tasks;
        private List<Categories>? categories;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            savings = new List<Savings>()
            {
                new Savings()
                {
                    SID = 1,
                    SGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 245.5M,
                    SOnWhat = "OnWhat1",
                    STime = new DateTime(2024, 1, 5, 23, 55, 1),
                    SWhere = "Where1",
                },
                new Savings()
                {
                    SID = 2,
                    SGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 2135.9M,
                    SOnWhat = "OnWhat2",
                    STime = new DateTime(2024, 1, 30, 23, 55, 1),
                    SWhere = "Where2",
                },
                new Savings()
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

            tasks = new List<Tasks>()
            {
                new Tasks()
                {
                    TID = 1,
                    TGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TBudget = 245,
                    TName = "Name1",
                    TTime = new DateTime(2024, 1, 5, 23, 55, 1),
                    TLocalization = "Localization1",
                    TStatus = TaskEnum.NotStarted,
                },
                new Tasks()
                {
                    TID = 2,
                    TGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TBudget = 2135,
                    TName = "Name2",
                    TTime = new DateTime(2024, 1, 30, 23, 55, 1),
                    TLocalization = "Localization2",
                    TStatus = TaskEnum.OnGoing,
                },
                new Tasks()
                {
                    TID = 3,
                    TGID = new Guid("20dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TBudget = 4455,
                    TName = "Name3",
                    TTime = new DateTime(2023, 12, 15, 23, 55, 1),
                    TLocalization = "Localization3",
                    TStatus = TaskEnum.OnGoing,
                },
            };

            categories = new List<Categories>()
            {
                new Categories()
                {
                    CID = 1,
                    CGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 1",
                    CBudget = 2050,
                    CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Categories()
                {
                    CID = 2,
                    CGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 2",
                    CBudget = 2060,
                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Categories()
                {
                    CID = 3,
                    CGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 3",
                    CBudget = 2070,
                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 6, 21, 30, 0)
                },
                new Categories()
                {
                    CID = 4,
                    CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 44,
                    CName = "Nazwa 4",
                    CBudget = 2050,
                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 4, 21, 30, 0)
                },
            };

            context.Setup(x => x.Savings).Returns(savings.AsQueryable());
            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.Categories).Returns(categories.AsQueryable());
        }

        //GetSavingBarChart
        [Test]
        public void TestStatsController_GetSavingBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
        {
            //Arrange
            var controller = new StatsController(context.Object);

            //Act
            var result = controller.GetSavingBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

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
            var controller = new StatsController(context.Object);

            //Act
            var result = controller.GetSavingBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 14, 23, 59, 59));

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
            var controller = new StatsController(context.Object);

            //Act
            var result = controller.GetSavingBarChart(new DateTime(2023, 12, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

            //Assert
            ClassicAssert.AreEqual(2, result?.Labels?.Count);
            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

            ClassicAssert.AreEqual("Oszczędności", result?.Datasets.Label);
            ClassicAssert.AreEqual(4455.1M, result?.Datasets?.Data[0]);
            ClassicAssert.AreEqual(2381.4M, result?.Datasets?.Data[1]);
        }

        //GetMoneySpendedFromTaskBarChart
        [Test]
        public void TestStatsController_GetMoneySpendedFromTaskBarChart_TakeSavingsOnlyFromJanuary2024_ShouldReturnDataFromSavingsFromJanuary2024()
        {
            //Arrange
            var controller = new StatsController(context.Object);

            //Act
            var result = controller.GetMoneySpendedFromTaskBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

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
            var controller = new StatsController(context.Object);

            //Act
            var result = controller.GetMoneySpendedFromTaskBarChart(new DateTime(2024, 1, 1, 0, 0, 0), new DateTime(2024, 1, 14, 23, 59, 59));

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
            var controller = new StatsController(context.Object);

            //Act
            var result = controller.GetMoneySpendedFromTaskBarChart(new DateTime(2023, 12, 1, 0, 0, 0), new DateTime(2024, 1, 31, 23, 59, 59));

            //Assert
            ClassicAssert.AreEqual(2, result?.Labels?.Count);
            ClassicAssert.AreEqual("2023-12", result?.Labels[0]);
            ClassicAssert.AreEqual("2024-1", result?.Labels[1]);

            ClassicAssert.AreEqual("Wydane pieniądze na zadania", result?.Datasets.Label);
            ClassicAssert.AreEqual(4455M, result?.Datasets?.Data[0]);
            ClassicAssert.AreEqual(2380M, result?.Datasets?.Data[1]);
        }

        //[Test]
        //public void TestCategoriesController_AddCategory_CategoryExistButErrorIsThrow_ShouldThrowException()
        //{
        //    //Arrange
        //    var controller = new CategoriesController(context.Object, mapper.Object);

        //    var category = new CategoriesViewModel()
        //    {
        //        CID = 2,
        //        CGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
        //    };

        //    //Act
        //    //Assert
        //    Assert.Throws<Exception>(() => controller.Save(category));
        //}

        //[Test]
        //public void TestCategoriesController_AddCategory_CategoryExist_ShouldModifyCategory()
        //{
        //    //Arrange
        //    var controller = new CategoriesController(context.Object, mapper.Object);

        //    var category = new CategoriesViewModel()
        //    {
        //        CID = 4,
        //        CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
        //        CUID = 44,
        //        CName = "Nazwa 5",
        //        CBudget = 2060,
        //        CStartDate = new DateTime(2023, 12, 5, 21, 30, 0),
        //        CEndDate = new DateTime(2023, 12, 12, 21, 30, 0)
        //    };

        //    //Act
        //    controller.Save(category);

        //    //Assert
        //    ClassicAssert.AreEqual(4, categories.Count);
        //    ClassicAssert.AreEqual("Nazwa 5", categories[3].CName);
        //    ClassicAssert.AreEqual(2060, categories[3].CBudget);
        //    ClassicAssert.AreEqual(new DateTime(2023, 12, 5, 21, 30, 0), categories[3].CStartDate);
        //    ClassicAssert.AreEqual(new DateTime(2023, 12, 12, 21, 30, 0), categories[3].CEndDate);
        //}

        ////Delete
        //[Test]
        //public void TestCategoriesController_DeleteCategory_CategoryNotFound_ShouldThrowException()
        //{
        //    //Arrange
        //    var controller = new CategoriesController(context.Object, mapper.Object);

        //    //Act
        //    //Assert
        //    Assert.Throws<Exception>(() => controller.Delete(Guid.Empty));
        //}

        //[Test]
        //public void TestCategoriesController_DeleteCategory_CategoryHasTasks_ShouldThrowException()
        //{
        //    //Arrange
        //    var controller = new CategoriesController(context.Object, mapper.Object);

        //    //Act
        //    //Assert
        //    Assert.Throws<Exception>(() => controller.Delete(Guid.Empty));
        //}

        //[Test]
        //public void TestCategoriesController_DeleteCategory_CategoryIsFound_ShouldDeleteCategory()
        //{
        //    //Arrange
        //    var controller = new CategoriesController(context.Object, mapper.Object);

        //    //Act
        //    controller.Delete(categories[2].CGID);

        //    //Assert
        //    ClassicAssert.AreEqual(3, categories.Count);
        //}
    }
}