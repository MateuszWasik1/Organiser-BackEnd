using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels;
using Organiser.CQRS.Resources.Categories.Commands;
using Organiser.CQRS.Resources.Categories.Queries;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestCategoriesController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestCategoriesController_Get_ShouldDispatch_GetCategoriesQuery()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.Get(new DateTime());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetCategoriesQuery, List<CategoriesViewModel>>(It.IsAny<GetCategoriesQuery>()), Times.Once);
        }

        [Test]
        public void TestCategoriesController_GetCategoriesForFilter_ShouldDispatch_GetCategoriesForFilterQuery()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.GetCategoriesForFilter();

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetCategoriesForFilterQuery, List<CategoriesForFiltersViewModel>>(It.IsAny<GetCategoriesForFilterQuery>()), Times.Once);
        }

        [Test]
        public void TestAccountsController_Save_ShouldDispatch_SaveCategoriesCommand()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.Save(new CategoriesViewModel());

            //Assert

            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveCategoriesCommand>()), Times.Once);
        }

        [Test]
        public void TestAccountsController_Save_ShouldDispatch_DeleteCategoriesCommand()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.Delete(new Guid());

            //Assert

            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<DeleteCategoriesCommand>()), Times.Once);
        }
    }
}