using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Savings.Commands;
using Organiser.Core.CQRS.Resources.Savings.Handlers;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Savings
{
    [TestFixture]
    public class TestDeleteSavingCommandHandler
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
                }
            };

            context.Setup(x => x.Savings).Returns(savings.AsQueryable());

            context.Setup(x => x.DeleteSaving(It.IsAny<Cores.Entities.Savings>())).Callback<Cores.Entities.Savings>(saving => savings.Remove(saving));
        }

        [Test]
        public void TestDeleteSavingCommandHandler_DeleteSaving_SavingNotFound_ShouldThrowException()
        {
            //Arrange
            var command = new DeleteSavingCommand() { SGID = Guid.Empty };
            var handler = new DeleteSavingCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteSavingCommandHandler_DeleteSaving_SavingIsFound_ShouldDeleteSaving()
        {
            //Arrange
            var command = new DeleteSavingCommand() { SGID = savings[1].SGID };
            var handler = new DeleteSavingCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, savings.Count);
        }
    }
}
