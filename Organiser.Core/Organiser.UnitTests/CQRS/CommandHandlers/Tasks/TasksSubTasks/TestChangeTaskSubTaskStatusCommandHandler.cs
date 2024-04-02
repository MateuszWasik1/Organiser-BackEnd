using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Bugs.Bugs.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.TasksSubTasks
{
    [TestFixture]
    public class TestChangeTaskSubTaskStatusCommandHandler
    {
        private Mock<IDataBaseContext> context;

        public List<Cores.Entities.TasksSubTasks> subTasks;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            subTasks = new List<Cores.Entities.TasksSubTasks>()
            {
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 1,
                    TSTGID = new Guid("30dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTTGID = new Guid("31dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTUID = 1,
                    TSTStatus = SubTasksStatusEnum.NotStarted,
                },
                new Cores.Entities.TasksSubTasks()
                {
                    TSTID = 2,
                    TSTGID = new Guid("32dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTTGID = new Guid("33dd879c-ee2f-11db-8314-0800200c9a66"),
                    TSTUID = 1,
                    TSTStatus = SubTasksStatusEnum.Done,
                },
            };

            context.Setup(x => x.TasksSubTasks).Returns(subTasks.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.TasksSubTasks>())).Callback<Cores.Entities.TasksSubTasks>(subTask => subTasks[0] = subTask);
        }

        [Test]
        public void TestChangeTaskSubTaskStatusCommandHandler_SubTaskNotFound_ShouldThrowTasksSubTasksNotFoundExceptions()
        {
            //Arrange 
            var model = new TasksSubTasksChangeStatusViewModel()
            {
                TSTGID = new Guid("91dd879c-ee2f-11db-8314-0800200c9a66"),
                Status = SubTasksStatusEnum.OnGoing
            };

            var command = new ChangeTaskSubTaskStatusCommand() { Model = model };
            var handler = new ChangeTaskSubTaskStatusCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TasksSubTasksNotFoundExceptions>(() => handler.Handle(command));
        }

        [TestCase("30dd879c-ee2f-11db-8314-0800200c9a66", 2)]
        [TestCase("30dd879c-ee2f-11db-8314-0800200c9a66", 3)]
        [TestCase("32dd879c-ee2f-11db-8314-0800200c9a66", 1)]
        public void TestChangeTaskSubTaskStatusCommandHandler_SubTaskIsFound_ShouldChangeStatus_BasedOnInput(string SubTaskGID, int status)
        {
            //Arrange
            var model = new TasksSubTasksChangeStatusViewModel()
            {
                TSTGID = Guid.Parse(SubTaskGID),
                Status = (SubTasksStatusEnum) status,
            };

            var command = new ChangeTaskSubTaskStatusCommand() { Model = model };
            var handler = new ChangeTaskSubTaskStatusCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.IsTrue(subTasks.FirstOrDefault(x => x.TSTGID == Guid.Parse(SubTaskGID))?.TSTStatus == (SubTasksStatusEnum) status);
        }
    }
}