using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
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
        public void TestCategoriesController_GetCategory_ShouldDispatch_GetCategoryQuery()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.GetCategory(new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetCategoryQuery, CategoryViewModel>(It.IsAny<GetCategoryQuery>()), Times.Once);
        }

        [Test]
        public void TestCategoriesController_GetCategories_ShouldDispatch_GetCategoriesQuery()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.GetCategories(new DateTime());

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
        public void TestAccountsController_AddCategory_ShouldDispatch_AddCategoryCommand()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.AddCategory(new CategoryViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<AddCategoryCommand>()), Times.Once);
        }

        [Test]
        public void TestAccountsController_UpdateCategory_ShouldDispatch_UpdateCategoryCommand()
        {
            //Arrange
            var controller = new CategoriesController(dispatcher.Object);

            //Act
            controller.UpdateCategory(new CategoryViewModel());

            //Assert
            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<UpdateCategoryCommand>()), Times.Once);
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