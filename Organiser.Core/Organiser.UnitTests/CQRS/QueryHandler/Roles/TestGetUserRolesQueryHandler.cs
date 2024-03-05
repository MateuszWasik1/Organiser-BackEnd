using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Roles.Handlers;
using Organiser.Core.CQRS.Resources.Roles.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.QueryHandler.Roles
{
    [TestFixture]
    public class TestGetUserRolesQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;

        private List<User> users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            users = new List<User>()
            {
                new User()
                {
                    UID = 1,
                    URID = 1,
                },
                new User()
                {
                    UID = 2,
                    URID = 2,
                },
                new User()
                {
                    UID = 3,
                    URID = 3,
                },
            };

            context.Setup(x => x.User).Returns(users.AsQueryable());
        }

        [Test]
        public void TestGetUserRolesQueryHandler_UserNotFound_ShouldActAsNormalUser()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(9);

            var query = new GetUserRolesQuery();
            var handler = new GetUserRolesQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsTrue(result.IsUser);
            ClassicAssert.IsFalse(result.IsSupport);
            ClassicAssert.IsFalse(result.IsAdmin);
        }

        [Test]
        public void TestGetUserRolesQueryHandler_UserIsUser_ShouldActAsNormalUser()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(1);

            var query = new GetUserRolesQuery();
            var handler = new GetUserRolesQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsTrue(result.IsUser);
            ClassicAssert.IsFalse(result.IsSupport);
            ClassicAssert.IsFalse(result.IsAdmin);
        }

        [Test]
        public void TestGetUserRolesQueryHandler_UserIsSupport_ShouldActAsSupport()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(2);

            var query = new GetUserRolesQuery();
            var handler = new GetUserRolesQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsTrue(result.IsUser);
            ClassicAssert.IsTrue(result.IsSupport);
            ClassicAssert.IsFalse(result.IsAdmin);
        }

        [Test]
        public void TestGetUserRolesQueryHandler_UserIsAdmin_ShouldActAsAdmin()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(3);

            var query = new GetUserRolesQuery();
            var handler = new GetUserRolesQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsTrue(result.IsUser);
            ClassicAssert.IsTrue(result.IsSupport);
            ClassicAssert.IsTrue(result.IsAdmin);
        }
    }
}
