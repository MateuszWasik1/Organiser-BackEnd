using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Savings.Handlers;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Core.Exceptions.Savings;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;

namespace Organiser.UnitTests.CQRS.QueryHandler.Notes
{
    [TestFixture]
    public class TestGetSavingQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.Savings>? savings;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            mapper = new Mock<IMapper>();

            savings = new List<Cores.Entities.Savings>() 
            {
                new Cores.Entities.Savings()
                {
                    SID = 1,
                    SUID = 1,
                    SGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SWhere = "Where1",
                    STime = new DateTime(2024, 3, 18, 18, 36, 0),
                    SOnWhat = "OnWhat1",
                    SAmount = 234,
                },
                new Cores.Entities.Savings()
                {
                    SID = 2,
                    SUID = 1,
                    SGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SWhere = "Where2",
                    STime = new DateTime(2024, 3, 18, 18, 46, 0),
                    SOnWhat = "OnWhat2",
                    SAmount = 2342,
                },
            };

            context.Setup(x => x.Savings).Returns(savings.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Savings, SavingViewModel>(It.IsAny<Cores.Entities.Savings>()))
                .Returns((Cores.Entities.Savings saving) =>
                    new SavingViewModel()
                    {
                        SGID = saving.SGID,
                        SAmount = saving.SAmount,
                        SOnWhat = saving.SOnWhat,
                        STime = saving.STime,
                        SWhere = saving.SWhere,
                    }
                );
        }

        [Test]
        public void TestGetSavingQueryHandler_SavingNotFound_ShouldThrowSavingNotFoundException()
        {
            //Arrange
            var query = new GetSavingQuery() { SGID = new Guid() };
            var handler = new GetSavingQueryHandler(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<SavingNotFoundException>(() => handler.Handle(query));
        }

        [Test]
        public void TestGetSavingQueryHandler_SavingWasFound_ShouldReturnNote()
        {
            //Arrange
            var query = new GetSavingQuery() { SGID = savings[0].SGID };
            var handler = new GetSavingQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(savings[0].SGID, result.SGID);
            ClassicAssert.AreEqual(savings[0].SWhere, result.SWhere);
            ClassicAssert.AreEqual(savings[0].SAmount, result.SAmount);
            ClassicAssert.AreEqual(savings[0].SOnWhat, result.SOnWhat);
            ClassicAssert.AreEqual(savings[0].STime, result.STime);
        }
    }
}
