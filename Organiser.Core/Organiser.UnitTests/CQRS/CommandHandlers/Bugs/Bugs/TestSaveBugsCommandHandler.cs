using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Commands;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.Exceptions.Accounts;
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
        public void TestSaveBugsCommandHandler_AddBug_TitleLengthIs0_ShouldThrowBugTitleRequiredExceptions()
        {
            //Arrange
            var model = new BugViewModel()
            {
                BGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                BTitle = "",
                BText = "",
                BStatus = BugStatusEnum.New
            };

            var command = new SaveBugCommand() { Model = model };
            var handler = new SaveBugsCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugTitleRequiredExceptions>(() => handler.Handle(command));
        }

        [Test]
        public void TestSaveBugsCommandHandler_AddBug_TitleLengthIsOver200_ShouldThrowBugTitleMax200Exceptions()
        {
            //Arrange
            var model = new BugViewModel()
            {
                BGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                BTitle = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec qua",
                BText = "",
                BStatus = BugStatusEnum.New
            };

            var command = new SaveBugCommand() { Model = model };
            var handler = new SaveBugsCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugTitleMax200Exceptions>(() => handler.Handle(command));
        }

        [Test]
        public void TestSaveBugsCommandHandler_AddBug_TextLengthIs0_ShouldThrowBugTextRequiredExceptions()
        {
            //Arrange
            var model = new BugViewModel()
            {
                BGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                BTitle = "1",
                BText = "",
                BStatus = BugStatusEnum.New
            };

            var command = new SaveBugCommand() { Model = model };
            var handler = new SaveBugsCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugTextRequiredExceptions>(() => handler.Handle(command));
        }

        [Test]
        public void TestSaveBugsCommandHandler_AddBug_TextLengthIsOver4000_ShouldThrowBugTextMax4000Exceptions()
        {
            //Arrange
            var model = new BugViewModel()
            {
                BGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                BTitle = "1",
                BText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibulum volutpat pretium libero. Cras id dui. Aenean ut eros et nisl sagittis vestibulum. Nullam nulla eros, ultricies sit amet, nonummy id, imperdiet feugiat, pede. Sed lectus. Donec mollis hendrerit risus. Phasellus nec sem in justo pellentesque facilisis. Etiam imperdiet imperdiet orci. Nunc nec neque. Phasellus leo dolor, tempus non, auctor et, hendrerit quis, nisi. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo. Maecenas malesuada. Praesent congue erat at massa. Sed cursus turpis vitae tortor. Donec posuere vulputate arcu. Phasellus accumsan cursus velit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed aliquam, nisi quis porttitor congue, elit erat euismod orci, ac placerat dolor lectus quis orci. Phasellus consectetuer vestibulum elit. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Vestibulum fringilla pede sit amet augue. In turpis. Pellentesque posuere. Praesent turpis. Aenean posuere, tortor sed cursus feugiat, nunc augue blandit nunc, eu sollicitudin urna dolor sagittis lacus. Donec elit libero, sodales nec, volutpat a, suscipit non, turpis. Nullam sagittis. Suspendisse pulvinar, augue ac venenatis condimentum, sem libero volutpat nibh, nec pellentesque velit pede quis nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce id purus. Ut varius tincidunt libero. Phasellus dolor. Maecenas vestibulum mollis diam. Pellentesque ut neque. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In dui magna, posuere eget, vestibulum et, tempor auctor, justo. In ac felis quis tortor malesuada pretium. Pellentesque auctor neque nec urna. Proin sapien ipsum, porta a, auctor quis, euismod ut, mi. Aenean viverra rhoncus pede. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Ut non enim eleifend felis pretium feugiat. Vivamus quis mi. Phasellus a est. Phase",
                BStatus = BugStatusEnum.New
            };

            var command = new SaveBugCommand() { Model = model };
            var handler = new SaveBugsCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<BugTextMax4000Exceptions>(() => handler.Handle(command));
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
