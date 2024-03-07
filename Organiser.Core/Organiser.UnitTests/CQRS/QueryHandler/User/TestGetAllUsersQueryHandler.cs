using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.User.Handlers;
using Organiser.Core.CQRS.Resources.User.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.UserViewModels;

namespace Organiser.UnitTests.CQRS.QueryHandler.User
{
    [TestFixture]
    public class TestGetAllUsersQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.User>? users;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            mapper = new Mock<IMapper>();

            users = new List<Cores.Entities.User>()
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    UGID = new Guid("b189857a-bf45-4c25-9644-f2408351d328"),
                    URID = 3,
                    UFirstName = "UFirstName1",
                    ULastName = "ULastName1",
                    UUserName = "UUserName1",
                    UEmail = "UEmail1",
                    UPhone = "UPhone1",
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("c189857a-bf45-4c25-9644-f2408351d328"),
                    URID = 1,
                    UFirstName = "UFirstName2",
                    ULastName = "ULastName2",
                    UUserName = "UUserName2",
                    UEmail = "UEmail2",
                    UPhone = "UPhone2",
                },
            };

            context.Setup(x => x.User).Returns(users.AsQueryable());
            context.Setup(x => x.AllUsers).Returns(users.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.User, UsersAdminViewModel>(It.IsAny<Cores.Entities.User>())).Returns(new UsersAdminViewModel()
            {
                UID = users[0].UID,
                UGID = users[0].UGID,
                URID = users[0].URID,
                UFirstName = users[0].UFirstName,
                ULastName = users[0].ULastName,
                UUserName = users[0].UUserName,
                UEmail = users[0].UEmail,
                UPhone = users[0].UPhone,
            });
        }

        [Test]
        public void TestGetAllUsersQueryHandler_GetAllUser_ShouldReturnAll2User()
        {
            //Arrange
            var query = new GetAllUsersQuery();
            var handler = new GetAllUsersQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(2, result.Count);
        }
    }
}
