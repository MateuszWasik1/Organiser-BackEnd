using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Bugs.Bugs
{
    [TestFixture]
    public class TestChangeBugStatusCommandHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;

        public List<Cores.Entities.Bugs> bugs;
        public List<Cores.Entities.BugsNotes> bugsNotes;
        public List<Cores.Entities.User> users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            bugs = new List<Cores.Entities.Bugs>()
            {
                new Cores.Entities.Bugs()
                {
                    BID = 1,
                    BGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 1,
                    BAUIDS = "99dd879c-ee2f-11db-8314-0800200c9a66",
                    BDate = new DateTime(2024, 3, 3, 17, 57, 59),
                    BTitle = "Błąd",
                    BText = "Treść błędu",
                    BStatus = BugStatusEnum.New,
                }
            };

            bugsNotes = new List<Cores.Entities.BugsNotes>();

            users = new List<Cores.Entities.User>()
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    UGID = new Guid("00dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 1,
                    UFirstName = "FirstName1",
                    ULastName = "LastName1",
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("01dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 2,
                    UFirstName = "NameS",
                    ULastName = "Support",
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    UGID = new Guid("02dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 3,
                    UFirstName = "NameA",
                    ULastName = "Admin",
                },
            };

            context.Setup(x => x.AllBugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.BugsNotes).Returns(bugsNotes.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs[0] = bug);
            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.BugsNotes>())).Callback<Cores.Entities.BugsNotes>(bugNote => bugsNotes.Add(bugNote));

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestChangeBugStatusCommandHandler_BugNotFound_ShouldThrowException()
        {
            //Arrange 
            var model = new ChangeBugStatusViewModel()
            {
                BGID = new Guid("31dd879c-ee2f-11db-8314-0800200c9a66"),
                Status = BugStatusEnum.Rejected
            };

            var command = new ChangeBugStatusCommand() { Model = model };
            var handler = new ChangeBugStatusCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestChangeBugStatusCommandHandler_UserNotFound_ShouldThrowException()
        {
            //Arrange 
            user.Setup(x => x.UID).Returns(4);

            var model = new ChangeBugStatusViewModel()
            {
                BGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                Status = BugStatusEnum.Rejected
            };

            var command = new ChangeBugStatusCommand() { Model = model };
            var handler = new ChangeBugStatusCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestChangeBugStatusCommandHandler_BugFound_ShouldChangeBugStatus_And_AddNewBugNote()
        {
            //Arrange 
            var model = new ChangeBugStatusViewModel()
            {
                BGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                Status = BugStatusEnum.Rejected
            };

            var command = new ChangeBugStatusCommand() { Model = model };
            var handler = new ChangeBugStatusCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, bugs.Count);
            ClassicAssert.AreEqual(1, bugsNotes.Count);

            ClassicAssert.AreEqual(BugStatusEnum.Rejected, bugs[0].BStatus);

            ClassicAssert.AreEqual(1, bugsNotes[0].BNUID);
            ClassicAssert.AreEqual($"Status został zmieniony na: \"Odrzucony\" przez użytkownika: FirstName1 LastName1", bugsNotes[0].BNText);
            ClassicAssert.AreEqual(false, bugsNotes[0].BNIsNewVerifier);
            ClassicAssert.AreEqual(true, bugsNotes[0].BNIsStatusChange);
            ClassicAssert.AreEqual(BugStatusEnum.Rejected, bugsNotes[0].BNChangedStatus);
        }

        [TestCase(2, "01dd879c-ee2f-11db-8314-0800200c9a66", "NameS", "Support")]
        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66", "NameA", "Admin")]
        public void TestChangeBugStatusCommandHandler_UserIsNotVerifier_UserIsAdminOrSupport_ShouldAddTwoBugsNotesAndCreateBugBAUIDS(int userRole, string ugid, string name, string surname)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var command = new ChangeBugStatusCommand()
            {
                Model = new ChangeBugStatusViewModel()
                {
                    BGID = bugs[0].BGID,
                    Status = BugStatusEnum.Accepted,
                }
            };
            var handler = new ChangeBugStatusCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, bugs.Count);
            ClassicAssert.IsTrue(bugs[0].BAUIDS.Contains(ugid));

            ClassicAssert.AreEqual(2, bugsNotes.Count);

            ClassicAssert.AreEqual(true, bugsNotes[0].BNIsNewVerifier);
            ClassicAssert.AreEqual($"Nowym weryfikującym jest: {name} {surname} {ugid}", bugsNotes[0].BNText);

            ClassicAssert.AreEqual(false, bugsNotes[1].BNIsNewVerifier);
            ClassicAssert.AreEqual($"Status został zmieniony na: \"Zaakceptowany\" przez użytkownika: {name} {surname}", bugsNotes[1].BNText);
        }

        [TestCase(2, "01dd879c-ee2f-11db-8314-0800200c9a66")]
        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66")]
        public void TestChangeBugStatusCommandHandler_UserIsNotVerifier_UserIsAdminOrSupport_ShouldAddTwoBugsNotesAndUpdateBugBAUIDS(int userRole, string ugid)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var command = new ChangeBugStatusCommand()
            {
                Model = new ChangeBugStatusViewModel()
                {
                    BGID = bugs[0].BGID,
                    Status = BugStatusEnum.Accepted,
                }
            };
            var handler = new ChangeBugStatusCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, bugs.Count);
            ClassicAssert.IsTrue(bugs[0].BAUIDS.Contains("99dd879c-ee2f-11db-8314-0800200c9a66"));
            ClassicAssert.IsTrue(bugs[0].BAUIDS.Contains(","));
            ClassicAssert.IsTrue(bugs[0].BAUIDS.Contains(ugid));
        }
    }
}
