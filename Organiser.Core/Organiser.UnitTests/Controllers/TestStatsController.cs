using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Stats.Queries;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.ViewModels.StatsViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestStatsController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestStatsController_GetMoneySpendedForCategoryBarChart_ShouldDispatch_GetMoneySpendedForCategoryBarChartQuery()
        {
            //Arrange
            var controller = new StatsController(dispatcher.Object);

            //Act
            controller.GetMoneySpendedForCategoryBarChart(new DateTime(), new DateTime(), new Guid());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetMoneySpendedForCategoryBarChartQuery, StatsBarChartViewModel>(It.IsAny<GetMoneySpendedForCategoryBarChartQuery>()), Times.Once);
        }

        [Test]
        public void TestStatsController_GetMoneySpendedFromTaskBarChart_ShouldDispatch_GetMoneySpendedFromTaskBarChartQuery()
        {
            //Arrange
            var controller = new StatsController(dispatcher.Object);

            //Act
            controller.GetMoneySpendedFromTaskBarChart(new DateTime(), new DateTime());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetMoneySpendedFromTaskBarChartQuery, StatsBarChartViewModel>(It.IsAny<GetMoneySpendedFromTaskBarChartQuery>()), Times.Once);
        }

        [Test]
        public void TestStatsController_GetSavingBarChart_ShouldDispatch_GetSavingBarChartQuery()
        {
            //Arrange
            var controller = new StatsController(dispatcher.Object);

            //Act
            controller.GetSavingBarChart(new DateTime(), new DateTime());

            //Assert
            dispatcher.Verify(x => x.DispatchQuery<GetSavingBarChartQuery, StatsBarChartViewModel>(It.IsAny<GetSavingBarChartQuery>()), Times.Once);
        }
    }
}