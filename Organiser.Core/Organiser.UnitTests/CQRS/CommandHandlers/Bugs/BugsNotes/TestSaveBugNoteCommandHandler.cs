using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Commands;
using Organiser.Core.CQRS.Resources.Bugs.BugsNotes.Handlers;
using Organiser.Core.Exceptions;
using Organiser.Core.Exceptions.Bugs;
using Organiser.Core.Models.ViewModels.BugsViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

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
                    URID = (int) RoleEnum.User
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("00dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Premium
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    UGID = new Guid("01dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Support,
                    UFirstName = "NameS",
                    ULastName = "Support",
                },
                new Cores.Entities.User()
                {
                    UID = 4,
                    UGID = new Guid("02dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = (int) RoleEnum.Admin,
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
        public void TestSaveBugNoteCommandHandler_TextIsEmpty_ShouldThrowBugsNotesTextMax4000Exception()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(1);

            var command = new SaveBugNoteCommand()
            {
                Model = new BugsNotesViewModel()
                {
                    BNBGID = new Guid("20dd879c-ee2f-11db-8314-0800200c9a66"),
                    BNText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibulum volutpat pretium libero. Cras id dui. Aenean ut eros et nisl sagittis vestibulum. Nullam nulla eros, ultricies sit amet, nonummy id, imperdiet feugiat, pede. Sed lectus. Donec mollis hendrerit risus. Phasellus nec sem in justo pellentesque facilisis. Etiam imperdiet imperdiet orci. Nunc nec neque. Phasellus leo dolor, tempus non, auctor et, hendrerit quis, nisi. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo. Maecenas malesuada. Praesent congue erat at massa. Sed cursus turpis vitae tortor. Donec posuere vulputate arcu. Phasellus accumsan cursus velit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed aliquam, nisi quis porttitor congue, elit erat euismod orci, ac placerat dolor lectus quis orci. Phasellus consectetuer vestibulum elit. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Vestibulum fringilla pede sit amet augue. In turpis. Pellentesque posuere. Praesent turpis. Aenean posuere, tortor sed cursus feugiat, nunc augue blandit nunc, eu sollicitudin urna dolor sagittis lacus. Donec elit libero, sodales nec, volutpat a, suscipit non, turpis. Nullam sagittis. Suspendisse pulvinar, augue ac venenatis condimentum, sem libero volutpat nibh, nec pellentesque velit pede quis nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce id purus. Ut varius tincidunt libero. Phasellus dolor. Maecenas vestibulum mollis diam. Pellentesque ut neque. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In dui magna, posuere eget, vestibulum et, tempor auctor, justo. In ac felis quis tortor malesuada pretium. Pellentesque auctor neque nec urna. Proin sapien ipsum, porta a, auctor quis, euismod ut, mi. Aenean viverra rhoncus pede. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Ut non enim eleifend felis pretium feugiat. Vivamus quis mi. Phasellus a est. Phase",
                }
            };
            var handler = new SaveBugNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugsNotesTextMax4000Exception>(() => handler.Handle(command));
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

        [TestCase(3, "01dd879c-ee2f-11db-8314-0800200c9a66", "NameS", "Support")]
        [TestCase(4, "02dd879c-ee2f-11db-8314-0800200c9a66", "NameA", "Admin")]
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

        [TestCase(3, "01dd879c-ee2f-11db-8314-0800200c9a66")]
        [TestCase(4, "02dd879c-ee2f-11db-8314-0800200c9a66")]
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
