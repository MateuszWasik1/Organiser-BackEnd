using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.User.Commands;
using Organiser.Core.CQRS.Resources.User.Handlers;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.CommandHandlers.User
{
    [TestFixture]
    public class TestDeleteUserCommandHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.User>? users;
        private List<Cores.Entities.Categories>? categories;
        private List<Cores.Entities.Tasks>? tasks;
        private List<Cores.Entities.TasksNotes>? tasksNotes;
        private List<Cores.Entities.Savings>? savings;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            users = new List<Cores.Entities.User> 
            {
                new Cores.Entities.User()
                {
                    UID = 1,
                    UGID = new Guid("98dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                },
                new Cores.Entities.User()
                {
                    UID = 2,
                    UGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                },
            };

            categories = new List<Cores.Entities.Categories>()
            {
                new Cores.Entities.Categories()
                {
                    CID = 1,
                    CGID = new Guid("00dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                },
                new Cores.Entities.Categories()
                {
                    CID = 2,
                    CGID = new Guid("01dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 1,
                },
                new Cores.Entities.Categories()
                {
                    CID = 2,
                    CGID = new Guid("02dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    CUID = 66,
                },
            };

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("03dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TGID = new Guid("04dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                },
                new Cores.Entities.Tasks()
                {
                    TID = 3,
                    TGID = new Guid("05dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 66,
                },
            };

            tasksNotes = new List<Cores.Entities.TasksNotes>()
            {
                new Cores.Entities.TasksNotes()
                {
                    TNID = 1,
                    TNGID = new Guid("06dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNUID = 1,
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 2,
                    TNGID = new Guid("07dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNUID = 1,
                },
                new Cores.Entities.TasksNotes()
                {
                    TNID = 3,
                    TNGID = new Guid("08dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TNUID = 66,
                },
            };

            savings = new List<Cores.Entities.Savings>()
            {
                new Cores.Entities.Savings()
                {
                    SID = 1,
                    SGID = new Guid("09dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                },
                new Cores.Entities.Savings()
                {
                    SID = 2,
                    SGID = new Guid("10dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                },
                new Cores.Entities.Savings()
                {
                    SID = 3,
                    SGID = new Guid("11dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 66,
                },
            };

            context.Setup(x => x.AllUsers).Returns(users.AsQueryable());
            context.Setup(x => x.AllCategories).Returns(categories.AsQueryable());
            context.Setup(x => x.AllTasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.AllTasksNotes).Returns(tasksNotes.AsQueryable());
            context.Setup(x => x.AllSavings).Returns(savings.AsQueryable());

            context.Setup(x => x.DeleteUser(It.IsAny<Cores.Entities.User>())).Callback<Cores.Entities.User>(user => users.Remove(user));
            context.Setup(x => x.DeleteCategory(It.IsAny<Cores.Entities.Categories>())).Callback<Cores.Entities.Categories>(category => categories.Remove(category));
            context.Setup(x => x.DeleteTask(It.IsAny<Cores.Entities.Tasks>())).Callback<Cores.Entities.Tasks>(task => tasks.Remove(task));
            context.Setup(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>())).Callback<Cores.Entities.TasksNotes>(taskNote => tasksNotes.Remove(taskNote));
            context.Setup(x => x.DeleteSaving(It.IsAny<Cores.Entities.Savings>())).Callback<Cores.Entities.Savings>(saving => savings.Remove(saving));
        }

        [Test]
        public void TestDeleteUserCommandHandler_UserNotFound_ShouldThrowException()
        {
            //Arrange
            var query = new DeleteUserCommand() { UGID = new Guid("69dacc1d-7bee-4635-9c4c-9404a4af80dd") };
            var handler = new DeleteUserCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(query));
        }

        [Test]
        public void TestDeleteUserCommandHandler_UserFound_ShouldDeleteUserAndAllHisData()
        {
            //Arrange
            var query = new DeleteUserCommand() { UGID = users[0].UGID };
            var handler = new DeleteUserCommandHandler(context.Object);

            //Act
            handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(1, users.Count);
            ClassicAssert.AreEqual(1, categories.Count);
            ClassicAssert.AreEqual(1, tasks.Count);
            ClassicAssert.AreEqual(1, tasksNotes.Count);
            ClassicAssert.AreEqual(1, savings.Count);

            context.Verify(x => x.DeleteUser(It.IsAny<Cores.Entities.User>()), Times.Once);
            context.Verify(x => x.DeleteCategory(It.IsAny<Cores.Entities.Categories>()), Times.Exactly(2));
            context.Verify(x => x.DeleteTask(It.IsAny<Cores.Entities.Tasks>()), Times.Exactly(2));
            context.Verify(x => x.DeleteTaskNotes(It.IsAny<Cores.Entities.TasksNotes>()), Times.Exactly(2));
            context.Verify(x => x.DeleteSaving(It.IsAny<Cores.Entities.Savings>()), Times.Exactly(2));
        }
    }
}
