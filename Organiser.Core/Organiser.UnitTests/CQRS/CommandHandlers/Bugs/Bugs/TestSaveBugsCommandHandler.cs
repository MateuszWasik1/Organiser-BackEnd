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
    public class TestSaveBugsCommandHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;

        public List<Cores.Entities.Bugs> bugs = new List<Cores.Entities.Bugs>() { };

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            context.Setup(x => x.Bugs).Returns(bugs.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Bugs>())).Callback<Cores.Entities.Bugs>(bug => bugs.Add(bug));

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestSaveBugsCommandHandler_AddBug_ShouldAddBug()
        {
            //Arrange
            var model = new BugViewModel()
            {
               BGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
               BTitle = "Title",
               BText = "Text",
               BStatus = BugStatusEnum.New
            };

            var command = new SaveBugCommand() { Model = model };
            var handler = new SaveBugsCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert

            ClassicAssert.AreEqual(1, bugs.Count);
            ClassicAssert.AreEqual(new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"), bugs[0].BGID);
            ClassicAssert.AreEqual("Title", bugs[0].BTitle);
            ClassicAssert.AreEqual("Text", bugs[0].BText);
            ClassicAssert.AreEqual(BugStatusEnum.New, bugs[0].BStatus);
        }
    }
}
