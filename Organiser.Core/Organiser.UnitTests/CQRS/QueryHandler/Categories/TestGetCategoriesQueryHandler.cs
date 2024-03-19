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
        public void TestGetCategoriesForFilterQueryHandler_GetCategoriesForFilterShouldReturn2Categories()
        {
            //Arrange
            var query = new GetCategoriesQuery() { Date = new DateTime(2024, 3, 5, 15, 14, 0) };
            var handler = new GetCategoriesQueryHandler(context.Object, mapper.Object);

            //Act
            handler.Handle(query);

            //Assert

            ClassicAssert.AreEqual(categories[0].CID, categoriesViewModel[0].CID);
            ClassicAssert.AreEqual(categories[0].CGID, categoriesViewModel[0].CGID);
            ClassicAssert.AreEqual(categories[0].CName, categoriesViewModel[0].CName);

            ClassicAssert.AreEqual(categories[1].CID, categoriesViewModel[1].CID);
            ClassicAssert.AreEqual(categories[1].CGID, categoriesViewModel[1].CGID);
            ClassicAssert.AreEqual(categories[1].CName, categoriesViewModel[1].CName);
        }
    }
}
