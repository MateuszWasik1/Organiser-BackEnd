using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.Exceptions.Categories;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.Cores.Services;
using Organiser.CQRS.Resources.Categories.Commands;
using Organiser.CQRS.Resources.Categories.Handlers;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Categories
{
    [TestFixture]
    public class TestAddCategoryCommandHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IUserContext>? user;

        private List<Cores.Entities.Categories>? categories;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            categories = new List<Cores.Entities.Categories>()
            {
                new Cores.Entities.Categories()
                {
                    CID = 1,
                    CGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 1",
                    CBudget = 2050,
                    CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Cores.Entities.Categories()
                {
                    CID = 2,
                    CGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 2",
                    CBudget = 2060,
                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Cores.Entities.Categories()
                {
                    CID = 3,
                    CGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                    CName = "Nazwa 3",
                    CBudget = 2070,
                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
                    CEndDate = new DateTime(2023, 12, 6, 21, 30, 0)
                },
                new Cores.Entities.Categories()
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

            context.Setup(x => x.Categories).Returns(categories.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Categories>())).Callback<Cores.Entities.Categories>(category => categories.Add(category));
        }

        [Test]
        public void TestAddCategoryCommandHandler_AddCategory_CNameIsEmpty_ShouldAddThrowCategoryNameRequiredException()
        {
            //Arrange
            var model = new CategoryViewModel()
            {
                CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                CName = "",
            };

            var command = new AddCategoryCommand() { Model = model };
            var handler = new AddCategoryCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<CategoryNameRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddCategoryCommandHandler_AddCategory_CNameIsOver300_ShouldAddThrowCategoryNameMax300Exception()
        {
            //Arrange
            var model = new CategoryViewModel()
            {
                CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                CName = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec p",
            };

            var command = new AddCategoryCommand() { Model = model };
            var handler = new AddCategoryCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<CategoryNameMax300Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddCategoryCommandHandler_AddCategory_CBudgetIsLowertThan0_ShouldAddThrowCategoryBudgetMin0Exception()
        {
            //Arrange
            var model = new CategoryViewModel()
            {
                CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                CName = "234",
                CBudget = -1,
            };

            var command = new AddCategoryCommand() { Model = model };
            var handler = new AddCategoryCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<CategoryBudgetMin0Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddCategoryCommandHandler_AddCategory_ShouldAddCategory()
        {
            //Arrange
            var model = new CategoryViewModel()
            {
                CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                CName = "Nazwa 5",
                CBudget = 2050,
                CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
                CEndDate = new DateTime(2023, 12, 9, 21, 30, 0)
            };

            var command = new AddCategoryCommand() { Model = model };
            var handler = new AddCategoryCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(5, categories.Count);
            ClassicAssert.AreEqual("Nazwa 5", categories[4].CName);
            ClassicAssert.AreEqual(2050, categories[4].CBudget);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), categories[4].CStartDate);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 9, 21, 30, 0), categories[4].CEndDate);
        }
    }
}