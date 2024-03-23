using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Handlers;
using Organiser.Core.Exceptions.Accounts;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;
using System.Linq;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Bugs.BugsNotes
{
    [TestFixture]
    public class TestSaveBugNoteCommandHandler
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
                    BGID = new Guid("33dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 1,
                    BAUIDS = "",
                    BTitle = "Bug 1 Title",
                    BText = "Bug 1",
                    BDate = new DateTime(2024, 3, 4, 14, 30 ,0),
                    BStatus = BugStatusEnum.New
                },
                new Cores.Entities.Bugs()
                {
                    BID = 2,
                    BGID = new Guid("98dd879c-ee2f-11db-8314-0800200c9a66"),
                    BUID = 2,
                    BAUIDS = "99dd879c-ee2f-11db-8314-0800200c9a66",
                    BTitle = "Bug 2 Title",
                    BText = "Bug 2",
                    BDate = new DateTime(2024, 3, 4, 14, 30 ,0),
                    BStatus = BugStatusEnum.New
                },
            };

            bugsNotes = new List<Cores.Entities.BugsNotes>();
            
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

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.AllBugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.BugsNotes).Returns(bugsNotes.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs[0] = bug);
            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.BugsNotes>())).Callback<Cores.Entities.BugsNotes>(bugNote => bugsNotes.Add(bugNote));

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestSaveBugNoteCommandHandler_TextIsEmpty_ShouldThrowBugsNotesTextRequiredException()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(1);

            var command = new SaveBugNoteCommand()
            {
                Model = new BugsNotesViewModel()
                {
                    BNBGID = new Guid("20dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNText = "",
                }
            };
            var handler = new SaveBugNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugsNotesTextRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestSaveBugNoteCommandHandler_CurrentUserNotFound_ShouldThrowUserNotFoundExceptions()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(7);

            var command = new SaveBugNoteCommand() { 
                Model =  new BugsNotesViewModel()
                {
                    BNBGID = new Guid("20dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNText = "NewText",
                }
            };
            var handler = new SaveBugNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<UserNotFoundExceptions>(() => handler.Handle(command));
        }

        [Test]
        public void TestSaveBugNoteCommandHandler_BugNotFound_ShouldThrowBugNotFoundExceptions()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(1);

            var command = new SaveBugNoteCommand()
            {
                Model = new BugsNotesViewModel()
                {
                    BNBGID = new Guid("20dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNText = "NewText",
                }
            };
            var handler = new SaveBugNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugNotFoundExceptions>(() => handler.Handle(command));
        }

        [TestCase(2, "01dd879c-ee2f-11db-8314-0800200c9a66", "NameS", "Support")]
        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66", "NameA", "Admin")]
        public void TestSaveBugNoteCommandHandler_UserIsNotVerifier_UserIsAdminOrSupport_ShouldAddTwoBugsNotesAndCreateBugBAUIDS(int userRole, string ugid, string name, string surname)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var command = new SaveBugNoteCommand()
            {
                Model = new BugsNotesViewModel()
                {
                    BNBGID = bugs[0].BGID,
                    BNText = "NewText",
                }
            };
            var handler = new SaveBugNoteCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(2, bugs.Count);
            ClassicAssert.IsTrue(bugs[0].BAUIDS.Contains(ugid));

            ClassicAssert.AreEqual(2, bugsNotes.Count);

            ClassicAssert.AreEqual(true, bugsNotes[0].BNIsNewVerifier);
            ClassicAssert.AreEqual($"Nowym weryfikującym jest: {name} {surname} {ugid}", bugsNotes[0].BNText);

            ClassicAssert.AreEqual(false, bugsNotes[1].BNIsNewVerifier);
            ClassicAssert.AreEqual("NewText", bugsNotes[1].BNText);
        }

        [TestCase(2, "01dd879c-ee2f-11db-8314-0800200c9a66")]
        [TestCase(3, "02dd879c-ee2f-11db-8314-0800200c9a66")]
        public void TestSaveBugNoteCommandHandler_UserIsNotVerifier_UserIsAdminOrSupport_ShouldAddTwoBugsNotesAndUpdateBugBAUIDS(int userRole, string ugid)
        {
            //Arrange
            user.Setup(x => x.UID).Returns(userRole);

            var command = new SaveBugNoteCommand()
            {
                Model = new BugsNotesViewModel()
                {
                    BNBGID = bugs[1].BGID,
                    BNText = "NewText",
                }
            };
            var handler = new SaveBugNoteCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(2, bugs.Count);
            ClassicAssert.IsTrue(bugs[1].BAUIDS.Contains("99dd879c-ee2f-11db-8314-0800200c9a66"));
            ClassicAssert.IsTrue(bugs[1].BAUIDS.Contains(","));
            ClassicAssert.IsTrue(bugs[1].BAUIDS.Contains(ugid));
        }
    }
}
