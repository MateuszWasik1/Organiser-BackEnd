using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Cores.Context;
using Organiser.CQRS.Resources.Categories.Handlers;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.UnitTests.CQRS.QueryHandler.Categories
{
    [TestFixture]
    public class TestGetCategoriesForFilterQueryHandler
    {
        private Mock<IDataBaseContext> context;

        private List<Cores.Entities.Categories> categories;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

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

            context.Setup(x => x.Categories).Returns(categories.AsQueryable());
        }

        [Test]
        public void TestGetCategoriesForFilterQueryHandler_GetCategoriesForFilter_ShouldReturn2Categories()
        {
            //Arrange
            var query = new GetCategoriesForFilterQuery();
            var handler = new GetCategoriesForFilterQueryHandler(context.Object);

            //Act
            var result = handler.Handle(query);

            //Assert

            ClassicAssert.AreEqual(categories[0].CID, result[0].CID);
            ClassicAssert.AreEqual(categories[0].CGID, result[0].CGID);
            ClassicAssert.AreEqual(categories[0].CName, result[0].CName);

            ClassicAssert.AreEqual(categories[1].CID, result[1].CID);
            ClassicAssert.AreEqual(categories[1].CGID, result[1].CGID);
            ClassicAssert.AreEqual(categories[1].CName, result[1].CName);
        }
    }
}
