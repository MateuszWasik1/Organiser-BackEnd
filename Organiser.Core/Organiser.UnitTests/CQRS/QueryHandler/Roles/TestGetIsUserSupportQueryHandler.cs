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
    public class TestGetIsUserSupportQueryHandler
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
        public void TestGetIsUserAdminQueryHandler_UserNotFound_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(9);

            var query = new GetIsUserSupportQuery();
            var handler = new GetIsUserSupportQueryHandler(context.Object, user.Object);

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

            var query = new GetIsUserSupportQuery();
            var handler = new GetIsUserSupportQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserIsSupport_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(2);

            var query = new GetIsUserSupportQuery();
            var handler = new GetIsUserSupportQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void TestGetIsUserAdminQueryHandler_UserIsAdmin_ShouldReturnFalse()
        {
            //Arrange
            user.Setup(x => x.UID).Returns(3);
            
            var query = new GetIsUserSupportQuery();
            var handler = new GetIsUserSupportQueryHandler(context.Object, user.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsFalse(result);
        }
    }
}