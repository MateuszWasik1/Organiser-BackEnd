using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Handlers;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Queries;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.QueryHandler.Bugs.BugsNotes
{
    [TestFixture]
    public class TestGetBugNotesQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;
        private Mock<IMapper> mapper;

        public List<Cores.Entities.Bugs> bugs;
        public List<Cores.Entities.BugsNotes> bugsNotes;
        public List<Cores.Entities.BugsNotes> allBugsNotes;
        public List<Cores.Entities.User> users;

        public List<BugsNotesViewModel> bugsNotesViewModel;

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

            bugsNotes = new List<Cores.Entities.BugsNotes>()
            {
                new Cores.Entities.BugsNotes()
                {
                    BNID = 1,
                    BNGID = new Guid("33dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNUID = 1,
                    BNBGID = bugs[0].BGID,
                    BNText = "Text 1",
                    BNChangedStatus = BugStatusEnum.New,
                    BNIsStatusChange = false,
                    BNIsNewVerifier = false,
                    BNDate = new DateTime(2024, 3, 4, 14, 30 ,0),
                },
                new Cores.Entities.BugsNotes()
                {
                    BNID = 2,
                    BNGID = new Guid("38dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNUID = 2,
                    BNBGID = bugs[0].BGID,
                    BNText = "Text 2",
                    BNChangedStatus = BugStatusEnum.New,
                    BNIsStatusChange = false,
                    BNIsNewVerifier = false,
                    BNDate = new DateTime(2024, 3, 4, 14, 30 ,0),
                },

                new Cores.Entities.BugsNotes()
                {
                    BNID = 3,
                    BNGID = new Guid("39dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNUID = 2,
                    BNBGID = bugs[0].BGID,
                    BNText = "Text 3",
                    BNChangedStatus = BugStatusEnum.New,
                    BNIsStatusChange = false,
                    BNIsNewVerifier = true,
                    BNDate = new DateTime(2024, 3, 4, 14, 30 ,0),
                },
            };

            allBugsNotes = new List<Cores.Entities.BugsNotes>
            {
                bugsNotes[0],
                bugsNotes[1],
                bugsNotes[2],
                new Cores.Entities.BugsNotes()
                {
                    BNID = 21,
                    BNGID = new Guid("34dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNUID = 3,
                    BNBGID =new Guid("35dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNText = "Text 4",
                    BNChangedStatus = BugStatusEnum.New,
                    BNIsStatusChange = false,
                    BNIsNewVerifier = false,
                }
            };

            users = new List<Cores.Entities.User>()
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    UGID = new Guid("00dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.User
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("01dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.User
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    UGID = new Guid("02dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Support
                },
                new Cores.Entities.User()
                {
                    UID = 4,
                    UGID = new Guid("03dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Admin
                },
            };

            bugsNotesViewModel = new List<BugsNotesViewModel>();

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.BugsNotes).Returns(bugsNotes.AsQueryable());
            context.Setup(x => x.AllBugsNotes).Returns(allBugsNotes.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs.Add(bug));

            user.Setup(x => x.UID).Returns(1);

            mapper.Setup(m => m.Map<Cores.Entities.BugsNotes, BugsNotesViewModel>(It.IsAny<Cores.Entities.BugsNotes>())).
                Callback((Cores.Entities.BugsNotes bugNote) =>
                    bugsNotesViewModel.Add(
                        new BugsNotesViewModel()
                        {
                            BNGID = bugNote.BNGID,
                            BNBGID = bugNote.BNBGID,
                            BNText = bugNote.BNText,
                            BNChangedStatus = bugNote.BNChangedStatus,
                            BNIsStatusChange = bugNote.BNIsStatusChange,
                            BNIsNewVerifier = bugNote.BNIsNewVerifier,
                            BNDate = bugNote.BNDate,
                        }
                    )
                );
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugNotesQueryHandler_UserIsAdminOrSupport_ShouldReturn_OneUserBugsNotes(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugNotesQuery() { BGID = new Guid("35dd879c-ee2f-11db-8314-0800200c9a66"), Skip = 0, Take = 10 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, result.Count);

            ClassicAssert.AreEqual("Text 4", bugsNotesViewModel[0].BNText);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugNotesQueryHandler_UserIsAdminOrSupport_ShouldReturn_ThreeUserBugsNotes(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugNotesQuery() { BGID = bugs[0].BGID, Skip = 0, Take = 10 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, result.Count);
            ClassicAssert.AreEqual(3, result.List.Count);

            ClassicAssert.AreEqual("Text 1", bugsNotesViewModel[0].BNText);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugNotesQueryHandler_UserIsAdminOrSupport_Skip0_Take1_ShouldReturn_OneUserBugsNotes(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugNotesQuery() { BGID = bugs[0].BGID, Skip = 0, Take = 1 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual("Text 1", bugsNotesViewModel[0].BNText);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TestGetBugNotesQueryHandler_UserIsAdminOrSupport_Skip1_Take1_ShouldReturn_OneUserBugsNotes(int userRole)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var query = new GetBugNotesQuery() { BGID = bugs[0].BGID, Skip = 1, Take = 1 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual("Text 2", bugsNotesViewModel[0].BNText);
        }

        [Test]
        public void TestGetBugNotesQueryHandler_UserIsUser_Skip_Take1_ShouldReturnUserBugsNotes()
        {
            //Arrange
            var query = new GetBugNotesQuery() { BGID = bugsNotes[0].BNBGID, Skip = 0, Take = 1 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual("Text 1", bugsNotesViewModel[0].BNText);
        }

        [Test]
        public void TestGetBugNotesQueryHandler_UserIsUser_Skip1_Take1_ShouldReturnUserBugsNotes()
        {
            //Arrange
            var query = new GetBugNotesQuery() { BGID = bugsNotes[0].BNBGID, Skip = 1, Take = 1 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual("Text 2", bugsNotesViewModel[0].BNText);
        }

        [Test]
        public void TestGetBugNotesQueryHandler_UserIsUser_ShouldReturnUserBugsNotes()
        {
            //Arrange
            var query = new GetBugNotesQuery() { BGID = bugsNotes[0].BNBGID, Skip = 0, Take = 10 };
            var handler = new GetBugNotesQueryHandler(context.Object, user.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual(2, result.List.Count);

            ClassicAssert.AreEqual("Text 1", bugsNotesViewModel[0].BNText);
        }
    }
}
