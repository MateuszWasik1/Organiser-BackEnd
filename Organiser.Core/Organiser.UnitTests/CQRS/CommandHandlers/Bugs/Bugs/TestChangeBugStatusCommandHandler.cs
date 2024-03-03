using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Models.Helpers;
using Organiser.Cores.Services;
using System;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Bugs.Bugs
{
    [TestFixture]
    public class TestChangeBugStatusCommandHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;

        public List<Cores.Entities.Bugs> bugs;
        public List<BugsNotes> bugsNotes;
        public List<User> users;

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
                    BAUIDS = "",
                    BDate = new DateTime(2024, 3, 3, 17, 57, 59),
                    BTitle = "Błąd",
                    BText = "Treść błędu",
                    BStatus = BugStatusEnum.New,
                }
            };

            bugsNotes = new List<BugsNotes>();

            users = new List<User>()
            {
                new User()
                {
                    UID = 1,
                    UFirstName = "FirstName",
                    ULastName = "LastName",
                }
            };

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());
            context.Setup(x => x.BugsNotes).Returns(bugsNotes.AsQueryable());
            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs[0] = bug);
            context.Setup(x => x.CreateOrUpdate(It.IsAny<BugsNotes>())).Callback<BugsNotes>(bugNote => bugsNotes.Add(bugNote));

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
            user.Setup(x => x.UID).Returns(2);

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
            ClassicAssert.AreEqual($"Status został zmieniony na: \"Odrzucony\" przez użytkownika: FirstName LastName", bugsNotes[0].BNText);
            ClassicAssert.AreEqual(false, bugsNotes[0].BNIsNewVerifier);
            ClassicAssert.AreEqual(true, bugsNotes[0].BNIsStatusChange);
            ClassicAssert.AreEqual(BugStatusEnum.Rejected, bugsNotes[0].BNChangedStatus);
        }
    }
}
