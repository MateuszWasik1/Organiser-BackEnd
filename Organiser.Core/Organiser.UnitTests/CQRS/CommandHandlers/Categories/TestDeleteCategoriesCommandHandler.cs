using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.Exceptions.Categories;
using Organiser.Cores.Context;
using Organiser.CQRS.Resources.Categories.Commands;
using Organiser.CQRS.Resources.Categories.Handlers;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Categories
{
    [TestFixture]
    public class TestDeleteCategoriesCommandHandler
    {
        private Mock<IDataBaseContext> context;

        private List<Cores.Entities.Categories> categories;
        private List<Cores.Entities.Tasks> tasks;

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

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TCGID = categories[1].CGID,
                }
            };

            context.Setup(x => x.Categories).Returns(categories.AsQueryable());
            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());

            context.Setup(x => x.DeleteCategory(It.IsAny<Cores.Entities.Categories>())).Callback<Cores.Entities.Categories>(category => categories.Remove(category));
        }

        [Test]
        public void TestDeleteCategoriesCommandHandler_CategoryNotFound_ShouldThrowException()
        {
            //Arrange
            var command = new DeleteCategoriesCommand() { CGID = Guid.NewGuid() };
            var handler = new DeleteCategoriesCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<CategoryNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteCategoriesCommandHandler_CategoryHasTasks_ShouldThrowException()
        {
            //Arrange
            var command = new DeleteCategoriesCommand() { CGID = categories[1].CGID };
            var handler = new DeleteCategoriesCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<CategoryHasTasksException>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteCategoriesCommandHandler_CategoryHasNotTasks_ShouldDeleteCategory()
        {
            //Arrange
            var command = new DeleteCategoriesCommand() { CGID = categories[0].CGID };
            var handler = new DeleteCategoriesCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.IsTrue(categories.Count() == 1);
            ClassicAssert.IsFalse(categories.Any(x => x.CGID == new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")));
        }
    }
}
