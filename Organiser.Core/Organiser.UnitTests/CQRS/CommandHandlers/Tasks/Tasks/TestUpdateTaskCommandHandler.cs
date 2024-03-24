﻿using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Commands;
using Organiser.Core.CQRS.Resources.Tasks.Tasks.Handlers;
using Organiser.Core.Exceptions.Tasks;
using Organiser.Core.Models.ViewModels.TasksViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.Enums;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Tasks.Tasks
{
    [TestFixture]
    public class TestUpdateTaskCommandHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Tasks>? tasks;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            tasks = new List<Cores.Entities.Tasks>()
            {
                new Cores.Entities.Tasks()
                {
                    TID = 1,
                    TGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TLocalization = "Lokalizacja 1",
                    TName = "Nazwa 1",
                    TBudget = 2050,
                    TStatus = TaskEnum.NotStarted,
                    TTime = new DateTime(2023, 12, 4, 21, 30, 0)
                },
                new Cores.Entities.Tasks()
                {
                    TID = 2,
                    TGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f6dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TLocalization = "Lokalizacja 2",
                    TName = "Nazwa 2",
                    TBudget = 2060,
                    TStatus = TaskEnum.OnGoing,
                    TTime = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Cores.Entities.Tasks()
                {
                    TID = 3,
                    TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TCGID = new Guid("f8dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    TUID = 1,
                    TLocalization = "Lokalizacja 3",
                    TName = "Nazwa 3",
                    TBudget = 2070,
                    TStatus = TaskEnum.Done,
                    TTime = new DateTime(2023, 12, 6, 21, 30, 0)
                }
            };

            context.Setup(x => x.Tasks).Returns(tasks.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Tasks>())).Callback<Cores.Entities.Tasks>(task => tasks[tasks.FindIndex(x => x.TID == task.TID)] = task);
        }

        [Test]
        public void TestUpdateTaskCommandHandler_UpdateTask_NameIsEmpty_ShouldThrowTaskNameRequiredException()
        {
            //Arrange
            var model = new TaskViewModel()
            {
                TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TName = "",
            };

            var command = new UpdateTaskCommand() { Model = model };
            var handler = new UpdateTaskCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskNameRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateTaskCommandHandler_UpdateTask_NameIsOver300_ShouldThrowTaskNameMax300Exception()
        {
            //Arrange
            var model = new TaskViewModel()
            {
                TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TName = "DummyText\r\nLorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec p",
            };

            var command = new UpdateTaskCommand() { Model = model };
            var handler = new UpdateTaskCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskNameMax300Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateTaskCommandHandler_UpdateTask_LocalizationIsOver300_ShouldThrowTaskLocalizationMax300Exception()
        {
            //Arrange
            var model = new TaskViewModel()
            {
                TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TName = "Name",
                TLocalization = "DummyText\r\nLorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec p",
            };

            var command = new UpdateTaskCommand() { Model = model };
            var handler = new UpdateTaskCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskLocalizationMax300Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateTaskCommandHandler_UpdateTask_BudgetIsUnder0_ShouldThrowTaskBudgetMin0Exception()
        {
            //Arrange
            var model = new TaskViewModel()
            {
                TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TName = "Name",
                TLocalization = "Localization",
                TBudget = -1,
            };

            var command = new UpdateTaskCommand() { Model = model };
            var handler = new UpdateTaskCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskBudgetMin0Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateTaskCommandHandler_UpdateTask_TaskDontExist_ShouldThrowTaskNotFoundException()
        {
            //Arrange
            var model = new TaskViewModel()
            {
                TGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TName = "Name",
                TLocalization = "Localization",
                TBudget = 11,
            };

            var command = new UpdateTaskCommand() { Model = model };
            var handler = new UpdateTaskCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<TaskNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateTaskCommandHandler_AddTask_TaskExist_ShouldModifyTask()
        {
            //Arrange
            var model = new TaskViewModel()
            {
                TGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                TLocalization = "Lokalizacja 5",
                TName = "Nazwa 5",
                TBudget = 2060,
                TStatus = TaskEnum.OnGoing,
                TTime = new DateTime(2023, 12, 5, 21, 30, 0)
            };

            var command = new UpdateTaskCommand() { Model = model };
            var handler = new UpdateTaskCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(3, tasks.Count);
            ClassicAssert.AreEqual("Nazwa 5", tasks[2].TName);
            ClassicAssert.AreEqual("Lokalizacja 5", tasks[2].TLocalization);
            ClassicAssert.AreEqual(2060, tasks[2].TBudget);
            ClassicAssert.AreEqual(TaskEnum.OnGoing, tasks[2].TStatus);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 5, 21, 30, 0), tasks[2].TTime);
        }
    }
}