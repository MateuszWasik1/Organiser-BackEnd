using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Accounts.Handlers;
using Organiser.Core.CQRS.Resources.Accounts.Queries;
using Organiser.Cores;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;

namespace Organiser.UnitTests.CQRS.QueryHandler.Accounts
{
    [TestFixture]
    public class TestLoginQueryHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IPasswordHasher<User>> hasher;
        private AuthenticationSettings authenticationSettings;

        private List<User> users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            hasher = new Mock<IPasswordHasher<User>>();
            authenticationSettings = new AuthenticationSettings();

            users = new List<User>()
            {
                new User()
                {
                    UID = 1,
                    UGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                    URID = 1,
                    UFirstName = "UFirstName1",
                    ULastName = "ULastName1",
                    UUserName = "Test1",
                    UPassword = "Password1",
                },
            };

            authenticationSettings = new AuthenticationSettings()
            {
                JWTKey = "JWT_KEY_FOR_LOGIN_QUERY_HANDLER_METHOD_UNIT_TESTS",
                JWTIssuer = "JWTIssuer",
                JWTExpiredDays = 1,
            };

            context.Setup(x => x.AllUsers).Returns(users.AsQueryable());

            hasher.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Failed);

        }

        [Test]
        public void TestLoginQueryHandler_NoUserNameFound_ShouldThrowException()
        {
            //Arrange 
            var query = new LoginQuery() { Username = "RandomUser", Password = users[0].UPassword };
            var handler = new LoginQueryHandler(context.Object, hasher.Object, authenticationSettings);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(query));
        }

        [Test]
        public void TestLoginQueryHandler_UserNameFound_PasswordIncorrect_ShouldThrowException()
        {
            //Arrange 
            var query = new LoginQuery() { Username = users[0].UUserName, Password = "RandomPassword" };
            var handler = new LoginQueryHandler(context.Object, hasher.Object, authenticationSettings);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(query));
        }

        [Test]
        public void TestLoginQueryHandler_UserNameAndPasswordCorrect_ShouldReturnJWTToken()
        {
            //Arrange 
            hasher.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);

            var query = new LoginQuery() { Username = users[0].UUserName, Password = users[0].UPassword };
            var handler = new LoginQueryHandler(context.Object, hasher.Object, authenticationSettings);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.IsInstanceOf<string>(result);
        }
    }
}
