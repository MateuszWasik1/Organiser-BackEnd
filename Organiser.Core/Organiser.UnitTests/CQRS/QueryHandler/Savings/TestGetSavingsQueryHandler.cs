using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Savings.Handlers;
using Organiser.Core.CQRS.Resources.Savings.Queries;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.SavingsViewModels;

namespace Organiser.UnitTests.CQRS.QueryHandler.Savings
{
    [TestFixture]
    public class TestGetSavingsQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.Savings>? savings;
        private List<SavingsViewModel>? savingsViewModel;

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
                    SGID = new Guid("f6dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 20,
                    SOnWhat = "Nazwa 3",
                    SWhere = "Lokalizacja 3",
                    STime = new DateTime(2024, 12, 5, 21, 30, 0)
                },
            };

            savingsViewModel = new List<SavingsViewModel>();

            context.Setup(x => x.Savings).Returns(savings.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Savings, SavingsViewModel>(It.IsAny<Cores.Entities.Savings>()))
                .Callback<Cores.Entities.Savings>((Cores.Entities.Savings saving) =>
                    savingsViewModel.Add(
                        new SavingsViewModel()
                        {
                            SID = saving.SID,
                            SGID = saving.SGID,
                        }
                    )
                );
        }

        [Test]
        public void TestSavingsController_GetAllDataForUser_Skip0_Take1_ShouldReturn_ThreeSavings()
        {
            //Arrange
            var query = new GetSavingsQuery() { Skip = 0, Take = 1 };
            var handler = new GetSavingsQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual(savings[0].SID, savingsViewModel[0].SID);
            ClassicAssert.AreEqual(savings[0].SGID, savingsViewModel[0].SGID);
        }

        [Test]
        public void TestSavingsController_GetAllDataForUser_Skip1_Take1_ShouldReturn_ThreeSavings()
        {
            //Arrange
            var query = new GetSavingsQuery() { Skip = 1, Take = 1 };
            var handler = new GetSavingsQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, result.Count);
            ClassicAssert.AreEqual(1, result.List.Count);

            ClassicAssert.AreEqual(savings[1].SID, savingsViewModel[0].SID);
            ClassicAssert.AreEqual(savings[1].SGID, savingsViewModel[0].SGID);
        }

        [Test]
        public void TestSavingsController_GetAllDataForUser_ShouldReturn_ThreeSavings()
        {
            //Arrange
            var query = new GetSavingsQuery() { Skip = 0, Take = 10};
            var handler = new GetSavingsQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(3, result.Count);
            ClassicAssert.AreEqual(3, result.List.Count);

            ClassicAssert.AreEqual(savings[0].SID, savingsViewModel[0].SID);
            ClassicAssert.AreEqual(savings[0].SGID, savingsViewModel[0].SGID);

            ClassicAssert.AreEqual(savings[1].SID, savingsViewModel[1].SID);
            ClassicAssert.AreEqual(savings[1].SGID, savingsViewModel[1].SGID);

            ClassicAssert.AreEqual(savings[2].SID, savingsViewModel[2].SID);
            ClassicAssert.AreEqual(savings[2].SGID, savingsViewModel[2].SGID);
        }
    }
}
