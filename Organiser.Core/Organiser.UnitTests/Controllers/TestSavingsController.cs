using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Cores.Context;
using Organiser.Cores.Controllers;
using Organiser.Cores.Entities;
using Organiser.Cores.Models.Enums;
using Organiser.Cores.Models.ViewModels;

namespace Organiser.UnitTests.Controllers
{
    [TestFixture]
    public class TestSavingsController
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Savings>? savings;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            mapper = new Mock<IMapper>();

            savings = new List<Savings>()
            {
                new Savings()
                {
                    SID = 1,
                    SGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 21.34M,
                    SOnWhat = "Nazwa 1",
                    SWhere = "Lokalizacja 1",
                    STime = new DateTime(2023, 12, 4, 21, 30, 0)
                },
                new Savings()
                {
                    SID = 2,
                    SGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 20,
                    SOnWhat = "Nazwa 2",
                    SWhere = "Lokalizacja 2",
                    STime = new DateTime(2023, 12, 5, 21, 30, 0)
                },
                new Savings()
                {
                    SID = 3,
                    SGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    SUID = 1,
                    SAmount = 50.55M,
                    SOnWhat = "Nazwa 3",
                    SWhere = "Lokalizacja 3",
                    STime = new DateTime(2023, 12, 6, 21, 30, 0)
                },
                new Savings()
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

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Savings>())).Callback<Savings>(saving => 
            { 
                var currentSaving = savings.FirstOrDefault(x => x.SID == saving.SID);

                if (currentSaving != null)
                    savings[savings.FindIndex(x => x.SID == currentSaving.SID)] = saving;
                else
                    savings.Add(saving);
            });

            context.Setup(x => x.DeleteSaving(It.IsAny<Savings>())).Callback<Savings>(saving => savings.Remove(saving));
        }

        //Get
        [Test]
        public void TestSavingsController_GetAllDataForUser_ShouldReturn3Savings()
        {
            //Arrange
            var controller = new SavingsController(context.Object, mapper.Object);

            //Act
            var result = controller.Get();

            //Assert
            ClassicAssert.AreEqual(4, result.Count);
        }

        //Post
        [Test]
        public void TestSavingsController_AddSaving_ShouldAddSaving()
        {
            //Arrange
            var controller = new SavingsController(context.Object, mapper.Object);

            var saving = new SavingsViewModel()
            {
                SID = 0,
                SGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                SUID = 44,
                SAmount = 23.17M,
                SOnWhat = "Nazwa 5",
                SWhere = "Lokalizacja 5",
                STime = new DateTime(2023, 12, 4, 21, 30, 0)
            };

            //Act
            controller.Save(saving);

            //Assert
            ClassicAssert.AreEqual(5, savings.Count);
            ClassicAssert.AreEqual("Nazwa 5", savings[4].SOnWhat);
            ClassicAssert.AreEqual("Lokalizacja 5", savings[4].SWhere);
            ClassicAssert.AreEqual(23.17M, savings[4].SAmount);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), savings[4].STime);
        }

        [Test]
        public void TestSavingsController_AddSaving_SavingExistButErrorIsThrow_ShouldThrowException()
        {
            //Arrange
            var controller = new SavingsController(context.Object, mapper.Object);

            var saving = new SavingsViewModel()
            {
                SID = 2,
                SGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
            };

            //Act
            //Assert
            Assert.Throws<Exception>(() => controller.Save(saving));
        }

        [Test]
        public void TestSavingsController_AddSaving_SavingExist_ShouldModifySaving()
        {
            //Arrange
            var controller = new SavingsController(context.Object, mapper.Object);

            var saving = new SavingsViewModel()
            {
                SID = 2,
                SGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                SUID = 2,
                SAmount = 23.17M,
                SOnWhat = "Nazwa 5",
                SWhere = "Lokalizacja 5",
                STime = new DateTime(2023, 12, 4, 21, 30, 0)
            };

            //Act
            controller.Save(saving);

            //Assert
            ClassicAssert.AreEqual(4, savings.Count);
            ClassicAssert.AreEqual("Nazwa 5", savings[1].SOnWhat);
            ClassicAssert.AreEqual("Lokalizacja 5", savings[1].SWhere);
            ClassicAssert.AreEqual(23.17M, savings[1].SAmount);
            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), savings[1].STime);
        }

        //Delete
        [Test]
        public void TestSavingsController_DeleteSaving_SavingNotFound_ShouldThrowException()
        {
            //Arrange
            var controller = new SavingsController(context.Object, mapper.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => controller.Delete(Guid.Empty));
        }

        [Test]
        public void TestSavingsController_DeleteSaving_SavingIsFound_ShouldDeleteSaving()
        {
            //Arrange
            var controller = new SavingsController(context.Object, mapper.Object);

            //Act
            controller.Delete(savings[2].SGID);

            //Assert
            ClassicAssert.AreEqual(3, savings.Count);
        }
    }
}