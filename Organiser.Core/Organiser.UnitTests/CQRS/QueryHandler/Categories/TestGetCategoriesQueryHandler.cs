using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Resources.Categories.Handlers;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.UnitTests.CQRS.QueryHandler.Categories
{
    [TestFixture]
    public class TestGetCategoriesQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IMapper> mapper;

        private List<Cores.Entities.Categories> categories;
        private List<CategoriesViewModel> categoriesViewModel;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            mapper = new Mock<IMapper>();

            categories = new List<Cores.Entities.Categories>()
            {
                new Cores.Entities.Categories()
                {
                    CID = 1,
                    CGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                    CUID = 1,
                    CBudget = 25,
                    CStartDate = new DateTime(2024, 3, 5, 15, 14, 0),
                    CEndDate =  new DateTime(2024, 3, 6, 15, 14, 0),
                    CName = "CategoryName1",
                },
                new Cores.Entities.Categories()
                {
                    CID = 2,
                    CGID = new Guid("31dd879c-ee2f-11db-8314-0800200c9a66"),
                    CUID = 1,
                    CBudget = 26,
                    CStartDate = new DateTime(2024, 3, 5, 15, 14, 0),
                    CEndDate =  new DateTime(2024, 3, 6, 15, 14, 0),
                    CName = "CategoryName2",
                },
                new Cores.Entities.Categories()
                {
                    CID = 3,
                    CGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                    CUID = 1,
                    CBudget = 26,
                    CStartDate = new DateTime(2024, 1, 5, 15, 14, 0),
                    CEndDate =  new DateTime(2024, 1, 6, 15, 14, 0),
                    CName = "CategoryName2",
                },
                new Cores.Entities.Categories()
                {
                    CID = 4,
                    CGID = new Guid("33dd879c-ee2f-11db-8314-0800200c9a66"),
                    CUID = 1,
                    CBudget = 25,
                    CStartDate = new DateTime(2024, 3, 5, 15, 14, 0),
                    CEndDate =  new DateTime(2024, 3, 6, 15, 14, 0),
                    CName = "CategoryName1",
                },
                new Cores.Entities.Categories()
                {
                    CID = 5,
                    CGID = new Guid("34dd879c-ee2f-11db-8314-0800200c9a66"),
                    CUID = 1,
                    CBudget = 26,
                    CStartDate = new DateTime(2024, 3, 5, 15, 14, 0),
                    CEndDate =  new DateTime(2024, 3, 6, 15, 14, 0),
                    CName = "CategoryName2",
                },
            };

            categoriesViewModel = new List<CategoriesViewModel>();

            context.Setup(x => x.Categories).Returns(categories.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Categories, CategoriesViewModel>(It.IsAny<Cores.Entities.Categories>()))
                .Callback((Cores.Entities.Categories categories) =>
                    categoriesViewModel.Add(
                        new CategoriesViewModel()
                        {
                            CID = categories.CID,
                            CGID = categories.CGID,
                            CName = categories.CName,
                            CBudgetCount = 0,
                        }
                    )
                ).Returns(new CategoriesViewModel()
                {
                    CID = 1,
                    CGID = new Guid(),
                    CName = "New name",
                    CBudgetCount = 0,
                });
        }

        [Test]
        public void TestGetCategoriesQueryHandler_GetCategories_TakeCategoriesForJanuary_ShouldReturn_OneCategory()
        {
            //Arrange
            var query = new GetCategoriesQuery() { Date = new DateTime(2024, 1, 1, 1, 1, 0), Skip = 0, Take = 10 };
            var handler = new GetCategoriesQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);

            ClassicAssert.AreEqual(categories[2].CID, categoriesViewModel[0].CID);
            ClassicAssert.AreEqual(categories[2].CGID, categoriesViewModel[0].CGID);
            ClassicAssert.AreEqual(categories[2].CName, categoriesViewModel[0].CName);
        }

        [Test]
        public void TestGetCategoriesQueryHandler_GetCategories_TakeCategoriesForMarch_ShouldReturn_FourCategories()
        {
            //Arrange
            var query = new GetCategoriesQuery() { Date = new DateTime(2024, 3, 5, 15, 14, 0), Skip = 0, Take = 10 };
            var handler = new GetCategoriesQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(4, result.Count);

            ClassicAssert.AreEqual(categories[0].CID, categoriesViewModel[0].CID);
            ClassicAssert.AreEqual(categories[0].CGID, categoriesViewModel[0].CGID);
            ClassicAssert.AreEqual(categories[0].CName, categoriesViewModel[0].CName);

            ClassicAssert.AreEqual(categories[1].CID, categoriesViewModel[1].CID);
            ClassicAssert.AreEqual(categories[1].CGID, categoriesViewModel[1].CGID);
            ClassicAssert.AreEqual(categories[1].CName, categoriesViewModel[1].CName);

            ClassicAssert.AreEqual(categories[3].CID, categoriesViewModel[2].CID);
            ClassicAssert.AreEqual(categories[3].CGID, categoriesViewModel[2].CGID);
            ClassicAssert.AreEqual(categories[3].CName, categoriesViewModel[2].CName);

            ClassicAssert.AreEqual(categories[4].CID, categoriesViewModel[3].CID);
            ClassicAssert.AreEqual(categories[4].CGID, categoriesViewModel[3].CGID);
            ClassicAssert.AreEqual(categories[4].CName, categoriesViewModel[3].CName);
        }

        [Test]
        public void TestGetCategoriesQueryHandler_GetCategories_TakeCategoriesForMarch_Skip0_Take_1_ShouldReturn_OneCategory()
        {
            //Arrange
            var query = new GetCategoriesQuery() { Date = new DateTime(2024, 3, 5, 15, 14, 0), Skip = 0, Take = 1 };
            var handler = new GetCategoriesQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(4, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count());

            ClassicAssert.AreEqual(categories[0].CID, categoriesViewModel[0].CID);
            ClassicAssert.AreEqual(categories[0].CGID, categoriesViewModel[0].CGID);
            ClassicAssert.AreEqual(categories[0].CName, categoriesViewModel[0].CName);
        }

        [Test]
        public void TestGetCategoriesQueryHandler_GetCategories_TakeCategoriesForMarch_Skip1_Take_1_ShouldReturn_OneCategory()
        {
            //Arrange
            var query = new GetCategoriesQuery() { Date = new DateTime(2024, 3, 5, 15, 14, 0), Skip = 1, Take = 1 };
            var handler = new GetCategoriesQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(4, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count());

            ClassicAssert.AreEqual(categories[1].CID, categoriesViewModel[0].CID);
            ClassicAssert.AreEqual(categories[1].CGID, categoriesViewModel[0].CGID);
            ClassicAssert.AreEqual(categories[1].CName, categoriesViewModel[0].CName);
        }
    }
}
