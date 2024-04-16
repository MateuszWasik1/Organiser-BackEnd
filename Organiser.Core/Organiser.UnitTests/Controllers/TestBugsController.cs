using Moq;
using NUnit.Framework;
using Organiser.Core.CQRS.Dispatcher;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Controllers;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestBugsController
    {
        private Mock<IDispatcher> dispatcher;

        [SetUp]
        public void SetUp() => dispatcher = new Mock<IDispatcher>();

        [Test]
        public void TestBugsController_GetBug_ShouldDispatch_GetBugQuery()
        {
            //Arrange
            var controller = new BugsController(dispatcher.Object);

            //Act
            controller.GetBug(new Guid());

            //Assert

            dispatcher.Verify(x => x.DispatchQuery<GetBugQuery, BugViewModel>(It.IsAny<GetBugQuery>()), Times.Once);
        }

        [Test]
        public void TestBugsController_GetBugs_ShouldDispatch_GetBugsQuery()
        {
            //Arrange
            var controller = new BugsController(dispatcher.Object);

            //Act
            controller.GetBugs(BugTypeEnum.My, 0 ,0);

            //Assert

            dispatcher.Verify(x => x.DispatchQuery<GetBugsQuery, GetBugsViewModel>(It.IsAny<GetBugsQuery>()), Times.Once);
        }

        [Test]
        public void TestBugsController_SaveBug_ShouldDispatch_SaveBugCommand()
        {
            //Arrange
            var controller = new BugsController(dispatcher.Object);

            //Act
            controller.SaveBug(new BugViewModel());

            //Assert

            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<SaveBugCommand>()), Times.Once);
        }

        [Test]
        public void TestBugsController_ChangeBugStatus_ShouldDispatch_ChangeBugStatusCommand()
        {
            //Arrange
            var controller = new BugsController(dispatcher.Object);

            //Act
            controller.ChangeBugStatus(new ChangeBugStatusViewModel());

            //Assert

            dispatcher.Verify(x => x.DispatchCommand(It.IsAny<ChangeBugStatusCommand>()), Times.Once);
        }
    }
}
