using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Handlers;
using Organiser.Core.Exceptions.Notes;
using Organiser.Core.Exceptions.Savings;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Savings
{
    [TestFixture]
    public class TestUpdateSavingCommandHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Savings>? savings;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            savings = new List<Cores.Entities.Savings>()
            {
                new Cores.Entities.Savings()
                {
                    SID = 1,
                    SGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 21.34M,
                    SOnWhat = "Nazwa 1",
                    SWhere = "Lokalizacja 1",
                    STime = new DateTime(2023, 12, 4, 21, 30, 0)
                },
                new Cores.Entities.Savings()
                {
                    SID = 2,
                    SGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 20,
                    SOnWhat = "Nazwa 2",
                    SWhere = "Lokalizacja 2",
                    STime = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Cores.Entities.Savings()
                {
                    SID = 3,
                    SGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 50.55M,
                    SOnWhat = "Nazwa 3",
                    SWhere = "Lokalizacja 3",
                    STime = new DateTime(2023, 12, 6, 21, 30, 0)
                },
                new Cores.Entities.Savings()
                {
                    SID = 4,
                    SGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 44,
                    SAmount = 23.17M,
                    SOnWhat = "Nazwa 4",
                    SWhere = "Lokalizacja 4",
                    STime = new DateTime(2023, 12, 4, 21, 30, 0)
                },
            };

            context.Setup(x => x.Savings).Returns(savings.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Savings>())).Callback<Cores.Entities.Savings>(saving => savings[savings.FindIndex(x => x.SID == saving.SID)] = saving);
        }

        [Test]
        public void TestUpdateSavingCommandHandler_SavingAmountLessThan0_ShouldThrowSavingAmountLessThan0Exception()
        {
            //Arrange
            var model = new SavingViewModel()
            {
                SGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                SAmount = -1,
                SOnWhat = "Nazwa 5",
                SWhere = "Lokalizacja 5",
                STime = new DateTime(2023, 12, 4, 21, 30, 0)
            };

            var command = new UpdateSavingCommand() { Model = model };
            var handler = new UpdateSavingCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<SavingAmountLessThan0Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateSavingCommandHandler_SavingNotFound_ShouldThrowSavingNotFoundException()
        {
            //Arrange
            var model = new SavingViewModel()
            {
                SGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
            };

            var command = new UpdateSavingCommand() { Model = model };
            var handler = new UpdateSavingCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<SavingNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateSavingCommandHandler_SavingExist_ShouldUpdateSaving()
        {
            //Arrange
            var model = new SavingViewModel()
            {
                SGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                SAmount = 23.17M,
                SOnWhat = "Nazwa 5",
                SWhere = "Lokalizacja 5",
                STime = new DateTime(2023, 12, 4, 21, 30, 0)
            };

            var command = new UpdateSavingCommand() { Model = model };
            var handler = new UpdateSavingCommandHandler(context.Object);


            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(4, savings.Count);
            ClassicAssert.AreEqual("Nazwa 5", savings[1].SOnWhat);
            ClassicAssert.AreEqual("Lokalizacja 5", savings[1].SWhere);
            ClassicAssert.AreEqual(23.17M, savings[1].SAmount);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), savings[1].STime);
        }
    }
}
