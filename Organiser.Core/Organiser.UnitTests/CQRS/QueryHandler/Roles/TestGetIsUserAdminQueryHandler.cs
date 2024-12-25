using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Roles.Handlers;
using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.QueryHandler.Roles
{
    [TestFixture]
    public class TestGetIsUserAdminQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;

        private List<Cores.Entities.User> users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            users = new List<Cores.Entities.User>()
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    URID = (int) RoleEnum.User,
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    URID = (int) RoleEnum.Premium,
                },
                new Cores.Entities.User()
                {
                    UID = 3,
                    URID = (int) RoleEnum.Support,
                },
                new Cores.Entities.User()
                {
                    UID = 4,
                    URID = (int) RoleEnum.Admin,
                },
            };

            context.Setup(x => x.User).Returns(users.AsQueryable());
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserNotFound_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(9);

            var query = new GetIsUserAdminQuery();
            var handler = new GetIsUserAdminQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserIsUser_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(1);

            var query = new GetIsUserAdminQuery();
            var handler = new GetIsUserAdminQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserIsPremium_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(2);

            var query = new GetIsUserAdminQuery();
            var handler = new GetIsUserAdminQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserIsSupport_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(3);

            var query = new GetIsUserAdminQuery();
            var handler = new GetIsUserAdminQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserIsAdmin_ShouldReturnTrue()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(4);

            var query = new GetIsUserAdminQuery();
            var handler = new GetIsUserAdminQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsTrue(result);
        }
    }
}
