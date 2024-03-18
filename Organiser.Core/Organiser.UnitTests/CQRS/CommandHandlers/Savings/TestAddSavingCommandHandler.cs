using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Handlers;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;
using Organiser.Cores.Services;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Savings
{
    [TestFixture]
    public class TestAddSavingCommandHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IUserContext>? user;

        private List<Cores.Entities.Savings>? savings;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

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

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Savings>())).Callback<Cores.Entities.Savings>(saving => savings.Add(saving));
        }

        [Test]
        public void TestAddSavingCommandHandler_AddSaving_ShouldAddSaving()
        {
            //Arrange
            var model = new SavingViewModel()
            {
                SGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                SAmount = 23.17M,
                SOnWhat = "Nazwa 5",
                SWhere = "Lokalizacja 5",
                STime = new DateTime(2023, 12, 4, 21, 30, 0)
            };

            var command = new AddSavingCommand() { Model = model };
            var handler = new AddSavingCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(5, savings.Count);
            ClassicAssert.AreEqual("Nazwa 5", savings[4].SOnWhat);
            ClassicAssert.AreEqual("Lokalizacja 5", savings[4].SWhere);
            ClassicAssert.AreEqual(23.17M, savings[4].SAmount);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), savings[4].STime);
        }
    }
}
