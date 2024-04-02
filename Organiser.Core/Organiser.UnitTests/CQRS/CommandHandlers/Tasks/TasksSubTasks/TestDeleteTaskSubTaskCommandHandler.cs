using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.TasksSubTasks
{
    [TestFixture]
    public class TestDeleteTaskSubTaskCommandHandler
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

            context.Setup(x => x.DeleteTaskSubTask(It.IsAny<Cores.Entities.TasksSubTasks>())).Callback<Cores.Entities.TasksSubTasks>(subTask => subTasks.Remove(subTask));
        }

        [Test]
        public void TestDeleteTaskSubTaskCommandHandler_SubTaskNotFound_ShouldThrowTasksSubTasksNotFoundExceptions()
        {
            //Arrange 
            var command = new DeleteTaskSubTaskCommand() { TSTGID = new Guid("91dd879c-ee2f-11db-8314-0800200c9a66") };
            var handler = new DeleteTaskSubTaskCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TasksSubTasksNotFoundExceptions>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteTaskSubTaskCommandHandler_SubTaskIsFound_ShouldDeleteSubTask()
        {
            //Arrange
            var command = new DeleteTaskSubTaskCommand() { TSTGID = subTasks[1].TSTGID };
            var handler = new DeleteTaskSubTaskCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, subTasks.Count);
            ClassicAssert.IsTrue(subTasks.Any(x => x.TSTGID == new Guid("30dd879c-ee2f-11db-8314-0800200c9a66")));
            ClassicAssert.IsFalse(subTasks.Any(x => x.TSTGID == new Guid("32dd879c-ee2f-11db-8314-0800200c9a66")));
        }
    }
}