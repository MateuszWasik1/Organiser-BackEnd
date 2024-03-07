using Moq;
using NUnit.Framework.Legacy;
using NUnit.Framework;
using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.CQRS.Resources.User.Handlers;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.UserViewModels;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.CommandHandlers.User
{
    [TestFixture]
    public class TestSaveUserCommandHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IUserContext>? user;

        private List<Cores.Entities.User>? users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            users = new List<Cores.Entities.User>
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    UGID = new Guid("98dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    URID =  1,
                    UFirstName = "OldName",
                    ULastName = "OldLastName",
                    UUserName = "OldUserName",
                    UEmail = "OldEmail",
                    UPhone = "OldPhone",
                },
            };

            context.Setup(x => x.User).Returns(users.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.User>())).Callback<Cores.Entities.User>(user =>
            {
                var currentUser = users.FirstOrDefault(x => x.UID == user.UID);

                users[users.FindIndex(x => x.UID == currentUser.UID)] = user;
            });

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestSaveUserCommandHandler_UserNotFound_ShouldThrowException()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(2);

            var query = new SaveUserCommand() { Model = new UserViewModel() };
            var handler = new SaveUserCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(query));
        }

        [Test]
        public void TestSaveUserByAdminCommandHandler_UserFound_ShouldUpdateUser()
        {
            //Arrange
            var model = new UserViewModel()
            {
                UFirstName = "NewName",
                ULastName = "NewLastName",
                UUserName = "NewUserName",
                UEmail = "NewEmail",
                UPhone = "NewPhone",
            };

            var query = new SaveUserCommand() { Model = model };
            var handler = new SaveUserCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual("NewName", users[0].UFirstName);
            ClassicAssert.AreEqual("NewLastName", users[0].ULastName);
            ClassicAssert.AreEqual("NewUserName", users[0].UUserName);
            ClassicAssert.AreEqual("NewEmail", users[0].UEmail);
            ClassicAssert.AreEqual("NewPhone", users[0].UPhone);
        }
    }
}
