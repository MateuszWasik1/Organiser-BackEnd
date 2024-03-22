using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.Exceptions.Accounts;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.QueryHandler.Bugs.Bugs
{
    [TestFixture]
    public class TestGetBugQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;
        private Mock<IMapper> mapper;

        public List<Cores.Entities.Bugs> bugs;
        public List<Cores.Entities.Bugs> allBugs;
        public List<Cores.Entities.User> users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();
            mapper = new Mock<IMapper>();

            bugs = new List<Cores.Entities.Bugs>()
            {
                new Cores.Entities.Bugs()
                {
                    BID = 1,
                    BGID = new Guid("33dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 1,
                    BAUIDS = "",
                    BTitle = "Bug 1 Title",
                    BText = "Bug 1",
                    BDate = new DateTime(2024, 3, 4, 14, 30 ,0),
                    BStatus = BugStatusEnum.New
                }
            };

            allBugs = new List<Cores.Entities.Bugs>
            {
                new Cores.Entities.Bugs()
                {
                    BID = 2,
                    BGID = new Guid("34dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 2,
                    BAUIDS = "",
                    BTitle = "Bug 2 Title",
                    BText = "Bug 2",
                    BDate = new DateTime(2024, 3, 4, 14, 30, 0),
                    BStatus = BugStatusEnum.InVerification,
                },
                bugs[0]
            };

            users = new List<Cores.Entities.User>()
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    URID = 1
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    URID = 2
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    URID = 3
                },
            };

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.AllBugs).Returns(allBugs.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs.Add(bug));

            user.Setup(x => x.UID).Returns(1);

            mapper.Setup(m => m.Map<Cores.Entities.Bugs, BugViewModel>(It.IsAny<Cores.Entities.Bugs>())).
                Returns((Cores.Entities.Bugs bug) => 
                    new BugViewModel()
                    {
                        BGID = bug.BGID,
                        BTitle = bug.BTitle,
                        BText = bug.BText,
                        BStatus = bug.BStatus,
                    }
                );
        }

        [Test]
        public void TestGetBugQueryHandler_UserTriedToLookInNotHisBug_UserIsNotAdmin_ShouldThrowBugNotFoundExceptions()
        {
            //Arrange
            var query = new GetBugQuery() { BGID  = allBugs[0].BGID };
            var handler = new GetBugQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<BugNotFoundExceptions>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetBugQueryHandler_UserTriedToLookInNotHisBug_UserIsAdmin_ShouldReturnBug()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(3);

            var query = new GetBugQuery() { BGID = allBugs[0].BGID };
            var handler = new GetBugQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(allBugs[0].BGID, result.BGID);
            ClassicAssert.AreEqual(allBugs[0].BTitle, result.BTitle);
            ClassicAssert.AreEqual(allBugs[0].BText, result.BText);
            ClassicAssert.AreEqual(allBugs[0].BStatus, result.BStatus);
        }

        [Test]
        public void TestGetBugQueryHandler_UserTriedToLookInNotHisBug_UserIsSupport_ShouldReturnBug()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(2);

            var query = new GetBugQuery() { BGID = allBugs[0].BGID };
            var handler = new GetBugQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(allBugs[0].BGID, result.BGID);
            ClassicAssert.AreEqual(allBugs[0].BTitle, result.BTitle);
            ClassicAssert.AreEqual(allBugs[0].BText, result.BText);
            ClassicAssert.AreEqual(allBugs[0].BStatus, result.BStatus);
        }

        [Test]
        public void TestGetBugQueryHandler_UserTriedToLookInHisBug_UserIsBugCreator_ShouldReturnBug()
        {
            //Arrange
            var query = new GetBugQuery() { BGID = bugs[0].BGID };
            var handler = new GetBugQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(bugs[0].BGID, result.BGID);
            ClassicAssert.AreEqual(bugs[0].BTitle, result.BTitle);
            ClassicAssert.AreEqual(bugs[0].BText, result.BText);
            ClassicAssert.AreEqual(bugs[0].BStatus, result.BStatus);
        }
    }
}
