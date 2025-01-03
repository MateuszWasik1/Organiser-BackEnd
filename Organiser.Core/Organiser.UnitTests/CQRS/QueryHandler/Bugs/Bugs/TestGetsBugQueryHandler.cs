﻿using AutoMapper;
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
                    BUID = 3,
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
                    BUID = 4,
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
                    BUID = 5,
                    BAUIDS = "02dd879c-ee2f-11db-8314-0800200c9a66, 03dd879c-ee2f-11db-8314-0800200c9a66",
                    BTitle = "Bug 4 Title",
                    BText = "Bug 4",
                    BDate = new DateTime(2024, 3, 4, 14, 30, 0),
                    BStatus = BugStatusEnum.New,
                },
                new Cores.Entities.Bugs()
                {
                    BID = 5,
                    BGID = new Guid("37dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 5,
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
                    BUID = 5,
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
                    URID = (int) RoleEnum.User,
                    UFirstName = "First1",
                    ULastName = "Last1",
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("01dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Premium,
                    UFirstName = "First2",
                    ULastName = "Last2",
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    UGID = new Guid("02dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Support,
                    UFirstName = "First3",
                    ULastName = "Last3",
                },
                new Cores.Entities.User()
                {
                    UID = 4,
                    UGID = new Guid("03dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Admin,
                    UFirstName = "First4",
                    ULastName = "Last4",
                },
            };

            bugsViewModel = new List<BugsViewModel>();

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.AllBugs).Returns(allBugs.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            user.Setup(x => x.UID).Returns(1);

            mapper.Setup(m => m.Map<Cores.Entities.Bugs, BugsViewModel>(It.IsAny<Cores.Entities.Bugs>()))
                .Callback((Cores.Entities.Bugs bug) =>
                    bugsViewModel.Add(
                        new BugsViewModel()
                        {
                            BID = bug.BID,
                            BGID = bug.BGID,
                            BUID = bug.BUID,
                            BTitle = bug.BTitle,
                            BText = bug.BText,
                            BStatus = bug.BStatus,
                            BDate = bug.BDate,
                        }
                    )
                );
        }

        [TestCase(3, 2)]
        [TestCase(4, 3)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_MY_ShouldReturnUserBugs(int userRole, int bugIndex)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.My, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual(bugIndex, bugsViewModel[0].BID);
        }

        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66")]
        [TestCase(4, "03dd879c-ee2f-11db-8314-0800200c9a66")]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_ImVerificator_ShouldReturnUserBugs(int userRole, string userGid)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);
            user.Setup(x => x.UGID).Returns(userGid);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.ImVerificator, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual(4, bugsViewModel[0].BID);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_Closed_ShouldReturnUserBugs(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.Closed, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual(2, result.List.Count);

            ClassicAssert.AreEqual(5, bugsViewModel[0].BID);
            ClassicAssert.AreEqual(6, bugsViewModel[1].BID);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_New_ShouldReturnNewBugs(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.New, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, bugsViewModel.Count);
            ClassicAssert.AreEqual(2, result.List.Count);
            ClassicAssert.AreEqual(2, result.Count);

            ClassicAssert.AreEqual(1, bugsViewModel[0].BID);
            ClassicAssert.AreEqual(4, bugsViewModel[1].BID);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_All_ShouldReturnAllBugs(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.All, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(6, result.List.Count);
            ClassicAssert.AreEqual(6, result.Count);

            ClassicAssert.AreEqual(allBugs.Count, bugsViewModel.Count);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_Unknown_ShouldReturnAllBugs(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = (BugTypeEnum) 5, Skip = 0 , Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(6, result.List.Count);
            ClassicAssert.AreEqual(6, result.Count);

            ClassicAssert.AreEqual(allBugs.Count, bugsViewModel.Count);
        }

        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66")]
        [TestCase(4, "03dd879c-ee2f-11db-8314-0800200c9a66")]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_ImVerificator_ShouldReturnUserBugs_With_BVerifiers(int userRole, string userGid)
        {
            //Arrange
            context.Setup(x => x.AllUsers).Returns(users.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Bugs, BugsViewModel>(It.IsAny<Cores.Entities.Bugs>()))
                .Returns((Cores.Entities.Bugs bug) =>
                    new BugsViewModel()
                    {
                        BID = bug.BID,
                        BGID = bug.BGID,
                        BUID = bug.BUID,
                        BTitle = bug.BTitle,
                        BText = bug.BText,
                        BStatus = bug.BStatus,
                        BDate = bug.BDate,
                    }
                );

            user.Setup(x => x.UID).Returns(userRole);
            user.Setup(x => x.UGID).Returns(userGid);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.ImVerificator, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.IsTrue(result.List.Any(x => x.BVerifiers.Contains($"{users[2].UFirstName} {users[2].ULastName} {users[2].UGID}")));
        }

        [TestCase(3, 1)]
        [TestCase(4, 1)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_All_Skip0_Take1_ShouldReturn_OneUserBugs(int userRole, int bugIndex)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.All, Skip = 0, Take = 1 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(6, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual(bugIndex, bugsViewModel[0].BID);
        }

        [TestCase(3, 2)]
        [TestCase(4, 2)]
        public void TestGetBugsQueryHandler_UserIsAdminOrSupport_BugType_Is_All_Skip1_Take1_ShouldReturn_OneUserBugs(int userRole, int bugIndex)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugsQuery() { BugType = BugTypeEnum.All, Skip = 1, Take = 1 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(6, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual(bugIndex, bugsViewModel[0].BID);
        }

        [Test]
        public void TestGetBugsQueryHandler_UserIsUser_BugType_Is_Unknown_ShouldReturnAllBugs()
        {
            //Arrange
            var query = new GetBugsQuery() { BugType = BugTypeEnum.My, Skip = 0, Take = 10 };
            var handler = new GetBugsQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, bugsViewModel.Count);
            ClassicAssert.AreEqual(1, bugsViewModel[0].BID);
            ClassicAssert.AreEqual(null, bugsViewModel[0].BVerifiers);
        }
    }
}
