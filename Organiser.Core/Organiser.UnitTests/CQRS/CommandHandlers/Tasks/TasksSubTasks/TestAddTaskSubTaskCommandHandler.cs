using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.TasksSubTasks.Handlers;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.TasksSubTasks
{
    [TestFixture]
    public class TestAddTaskSubTaskCommandHandler
    {
        private Mock<IDataBaseContext> context;
        private Mock<IUserContext> user;

        public List<Cores.Entities.Tasks> tasks;
        public List<Cores.Entities.TasksSubTasks> subTasks;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("28dd879c-ee2f-11db-8314-0800200c9a66"),
                    TUID = 1,
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TGID = new Guid("29dd879c-ee2f-11db-8314-0800200c9a66"),
                    TUID = 1,
                },
            };

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

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());
            context.Setup(x => x.TasksSubTasks).Returns(subTasks.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.TasksSubTasks>())).Callback<Cores.Entities.TasksSubTasks>(subTask => subTasks.Add(subTask));

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestAddTaskSubTaskCommandHandler_SubTaskTitleIsZero_ShouldThrowTaskSubTaskTitleRequiredException()
        {
            //Arrange 
            var model = new TasksAddSubTaskViewModel()
            {
                TSTGID = Guid.NewGuid(),
                TSTTGID = tasks[0].TGID,
                TSTTitle = "",
                TSTText = "",
            };

            var command = new AddTaskSubTaskCommand() { Model = model };
            var handler = new AddTaskSubTaskCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskSubTaskTitleRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskSubTaskCommandHandler_SubTaskTitleIsOver200_ShouldThrowTaskSubTaskTitleMax200Exception()
        {
            //Arrange 
            var model = new TasksAddSubTaskViewModel()
            {
                TSTGID = Guid.NewGuid(),
                TSTTGID = tasks[0].TGID,
                TSTTitle = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec qua",
                TSTText = "",
            };

            var command = new AddTaskSubTaskCommand() { Model = model };
            var handler = new AddTaskSubTaskCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskSubTaskTitleMax200Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskSubTaskCommandHandler_SubTaskTextIsZero_ShouldThrowTaskSubTaskTextRequiredException()
        {
            //Arrange 
            var model = new TasksAddSubTaskViewModel()
            {
                TSTGID = Guid.NewGuid(),
                TSTTGID = tasks[0].TGID,
                TSTTitle = "2",
                TSTText = "",
            };

            var command = new AddTaskSubTaskCommand() { Model = model };
            var handler = new AddTaskSubTaskCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskSubTaskTextRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskSubTaskCommandHandler_SubTaskTextIsOver2000_ShouldThrowTaskSubTaskTextMax2000Exception()
        {
            //Arrange 
            var model = new TasksAddSubTaskViewModel()
            {
                TSTGID = Guid.NewGuid(),
                TSTTGID = tasks[0].TGID,
                TSTTitle = "2",
                TSTText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibu",
            };

            var command = new AddTaskSubTaskCommand() { Model = model };
            var handler = new AddTaskSubTaskCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskSubTaskTextMax2000Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskSubTaskCommandHandler_TaskNotFound_ShouldThrowTaskNotFoundException()
        {
            //Arrange 
            var model = new TasksAddSubTaskViewModel()
            {
                TSTGID = Guid.NewGuid(),
                TSTTGID = Guid.NewGuid(),
                TSTTitle = "2",
                TSTText = "2",
            };

            var command = new AddTaskSubTaskCommand() { Model = model };
            var handler = new AddTaskSubTaskCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddTaskSubTaskCommandHandler_ShouldAddSubTask()
        {
            //Arrange
            var model = new TasksAddSubTaskViewModel()
            {
                TSTGID = Guid.NewGuid(),
                TSTTGID = tasks[0].TGID,
                TSTTitle = "2",
                TSTText = "2",
            };

            var command = new AddTaskSubTaskCommand() { Model = model };
            var handler = new AddTaskSubTaskCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(3, subTasks.Count);
            ClassicAssert.AreEqual(subTasks[2].TSTTGID, tasks[0].TGID);
            ClassicAssert.AreEqual(subTasks[2].TSTTitle, "2");
            ClassicAssert.AreEqual(subTasks[2].TSTText, "2");
        }
    }
}