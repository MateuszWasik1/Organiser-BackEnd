using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.QueryHandler.Bugs.Bugs
{
    [TestFixture]
    public class TestGetsBugQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;
        private Mock<IMapper> mapper;

        public List<Cores.Entities.Bugs> bugs;
        public List<Cores.Entities.Bugs> allBugs;
        public List<Cores.Entities.User> users;

        public List<BugsViewModel> bugsViewModel;

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
                bugs[0],
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
                new Cores.Entities.Bugs()
                {
                    BID = 3,
                    BGID = new Guid("35dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 3,
                    BAUIDS = "",
                    BTitle = "Bug 3 Title",
                    BText = "Bug 3",
                    BDate = new DateTime(2024, 3, 4, 14, 30, 0),
                    BStatus = BugStatusEnum.InDevelopment,
                },
                new Cores.Entities.Bugs()
                {
                    BID = 4,
                    BGID = new Guid("36dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 4,
                    BAUIDS = "01dd879c-ee2f-11db-8314-0800200c9a66,02dd879c-ee2f-11db-8314-0800200c9a66",
                    BTitle = "Bug 4 Title",
                    BText = "Bug 4",
                    BDate = new DateTime(2024, 3, 4, 14, 30, 0),
                    BStatus = BugStatusEnum.New,
                },
                new Cores.Entities.Bugs()
                {
                    BID = 5,
                    BGID = new Guid("37dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 4,
                    BAUIDS = "",
                    BTitle = "Bug 5 Title",
                    BText = "Bug 5",
                    BDate = new DateTime(2024, 3, 4, 14, 30, 0),
                    BStatus = BugStatusEnum.Rejected,
                },
                new Cores.Entities.Bugs()
                {
                    BID = 6,
                    BGID = new Guid("38dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 4,
                    BAUIDS = "",
                    BTitle = "Bug 6 Title",
                    BText = "Bug 6",
                    BDate = new DateTime(2024, 3, 4, 14, 30, 0),
                    BStatus = BugStatusEnum.Fixed,
                },
            };

            users = new List<Cores.Entities.User>()
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    UGID = new Guid("00dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 1
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("01dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 2
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    UGID = new Guid("02dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 3
                },
            };

            bugsViewModel = new List<BugsViewModel>();

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.AllBugs).Returns(allBugs.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs.Add(bug));

            user.Setup(x => x.UID).Returns(1);

            mapper.Setup(m => m.Map<Cores.Entities.Bugs, BugsViewModel>(It.IsAny<Cores.Entities.Bugs>())).
                Callback<Cores.Entities.Bugs>((Cores.Entities.Bugs bug) =>
                    bugsViewModel.Add(
                        new BugsViewModel()
                        {   
                            BID = bug.BID,
                            BGID = bug.BGID,
                            BUID = bug.BUID,
                            BTitle = bug.BTitle,
                            BText = bug.BText,
                            BStatus = bug.BStatus,
                            BAUIDS = bug.BAUIDS,
                            BDate = bug.BDate,
                        }
                    )
                );
        }

        [TestCase(2, 2)]
        [TestCase(3, 3)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_MY_ShouldReturnUserBugs(int userRole, int bugIndex)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.My };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);

            ClassicAssert.AreEqual(bugIndex, bugsViewModel[0].BID);
        }

        [TestCase(2, "01dd879c-ee2f-11db-8314-0800200c9a66")]
        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66")]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_ImVerificator_ShouldReturnUserBugs(int userRole, string userGid)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);
            user.Setup(x => x.UGID).Returns(userGid);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.ImVerificator };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);

            ClassicAssert.AreEqual(4, bugsViewModel[0].BID);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_Closed_ShouldReturnUserBugs(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.Closed };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result.Count);

            ClassicAssert.AreEqual(5, bugsViewModel[0].BID);
            ClassicAssert.AreEqual(6, bugsViewModel[1].BID);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_Unknown_ShouldReturnAllBugs(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = (BugTypeEnum) 3 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(allBugs.Count, bugsViewModel.Count);
        }

        [Test]
        public void TestGetBugsQueryHandler_UserIsUser_BugType_Is_Unknown_ShouldReturnAllBugs()
        {
            //Arrange
            var query = new GetBugsQuery() { BugType = BugTypeEnum.My };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, bugsViewModel.Count);
            ClassicAssert.AreEqual(1, bugsViewModel[0].BID);
        }
    }
}
