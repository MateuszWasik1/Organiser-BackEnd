using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.Exceptions.Categories;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.CQRS.Resources.Categories.Handlers;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.UnitTests.CQRS.QueryHandler.Notes
{
    [TestFixture]
    public class TestGetCategoryQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.Categories>? categories;

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
                    CUID = 1,
                    CGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CName = "Name1",
                    CStartDate = new DateTime(2024, 3, 18, 18, 36, 0),
                    CEndDate = new DateTime(2024, 3, 19, 18, 36, 0),
                    CBudget = 234,
                },
                new Cores.Entities.Categories()
                {
                    CID = 2,
                    CUID = 1,
                    CGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CName = "Name2",
                    CStartDate = new DateTime(2024, 3, 18, 18, 36, 0),
                    CEndDate = new DateTime(2024, 3, 19, 18, 36, 0),
                    CBudget = 2334,
                },
            };

            context.Setup(x => x.Categories).Returns(categories.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Categories, CategoryViewModel>(It.IsAny<Cores.Entities.Categories>()))
                .Returns((Cores.Entities.Categories category) =>
                    new CategoryViewModel()
                    {
                        CGID = category.CGID,
                        CName = category.CName,
                        CStartDate = category.CStartDate,
                        CEndDate = category.CEndDate,
                        CBudget = category.CBudget,
                        CBudgetCount = 0,
                    }
                );
        }

        [Test]
        public void TestGetCategoryQueryHandler_CategoryNotFound_ShouldThrowException()
        {
            //Arrange
            var query = new GetCategoryQuery() { CGID = new Guid() };
            var handler = new GetCategoryQueryHandler(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<CategoryNotFoundException>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetCategoryQueryHandler_CategoryWasFound_ShouldReturnCategory()
        {
            //Arrange
            var query = new GetCategoryQuery() { CGID = categories[0].CGID };
            var handler = new GetCategoryQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(categories[0].CGID, result.CGID);
            ClassicAssert.AreEqual(categories[0].CStartDate, result.CStartDate);
            ClassicAssert.AreEqual(categories[0].CEndDate, result.CEndDate);
            ClassicAssert.AreEqual(categories[0].CName, result.CName);
            ClassicAssert.AreEqual(categories[0].CBudget, result.CBudget);
        }
    }
}
