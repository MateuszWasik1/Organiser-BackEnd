//using AutoMapper;
//using Moq;
//using NUnit.Framework;
//using NUnit.Framework.Legacy;
//using Organiser.Cores.Context;
//using Organiser.Cores.Controllers;
//using Organiser.Cores.Entities;
//using Organiser.Cores.Models.Enums;
//using Organiser.Cores.Models.ViewModels;
//using Organiser.Cores.Services;

//namespace Organiser.UnitTests.Controllers
//{
//    [TestFixture]
//    public class TestCategoriesController
//    {
//        private Mock<IDataBaseContext>? context;
//        private Mock<IUserContext>? user;
//        private Mock<IMapper>? mapper;

//        private List<Categories>? categories;

//        [SetUp]
//        public void SetUp()
//        {
//            context = new Mock<IDataBaseContext>();
//            user = new Mock<IUserContext>();
//            mapper = new Mock<IMapper>();

//            categories = new List<Categories>()
//            {
//                new Categories()
//                {
//                    CID = 1,
//                    CGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    CUID = 1,
//                    CName = "Nazwa 1",
//                    CBudget = 2050,
//                    CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
//                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
//                },
//                new Categories()
//                {
//                    CID = 2,
//                    CGID = new Guid("f5dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    CUID = 1,
//                    CName = "Nazwa 2",
//                    CBudget = 2060,
//                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
//                    CEndDate = new DateTime(2023, 12, 5, 21, 30, 0)
//                },
//                new Categories()
//                {
//                    CID = 3,
//                    CGID = new Guid("f7dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    CUID = 1,
//                    CName = "Nazwa 3",
//                    CBudget = 2070,
//                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
//                    CEndDate = new DateTime(2023, 12, 6, 21, 30, 0)
//                },
//                new Categories()
//                {
//                    CID = 4,
//                    CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                    CUID = 44,
//                    CName = "Nazwa 4",
//                    CBudget = 2050,
//                    CStartDate = new DateTime(2023, 12, 1, 21, 30, 0),
//                    CEndDate = new DateTime(2023, 12, 4, 21, 30, 0)
//                },
//            };

//            context.Setup(x => x.Categories).Returns(categories.AsQueryable());

//            context.Setup(x => x.CreateOrUpdate(It.IsAny<Categories>())).Callback<Categories>(category => 
//            { 
//                var currentCategory = categories.FirstOrDefault(x => x.CID == category.CID);

//                if (currentCategory != null)
//                    categories[categories.FindIndex(x => x.CID == currentCategory.CID)] = category;
//                else
//                    categories.Add(category);
//            });

//            context.Setup(x => x.DeleteCategory(It.IsAny<Categories>())).Callback<Categories>(category => categories.Remove(category));
//        }

//        //Post
//        [Test]
//        public void TestCategoriesController_AddCategory_ShouldAddCategory()
//        {
//            //Arrange
//            var controller = new CategoriesController(context.Object, user.Object, mapper.Object);

//            var category = new CategoriesViewModel()
//            {
//                CID = 0,
//                CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                CUID = 44,
//                CName = "Nazwa 5",
//                CBudget = 2050,
//                CStartDate = new DateTime(2023, 12, 4, 21, 30, 0),
//                CEndDate = new DateTime(2023, 12, 9, 21, 30, 0)
//            };

//            //Act
//            controller.Save(category);

//            //Assert
//            ClassicAssert.AreEqual(5, categories.Count);
//            ClassicAssert.AreEqual("Nazwa 5", categories[4].CName);
//            ClassicAssert.AreEqual(2050, categories[4].CBudget);
//            ClassicAssert.AreEqual(new DateTime(2023, 12, 4, 21, 30, 0), categories[4].CStartDate);
//            ClassicAssert.AreEqual(new DateTime(2023, 12, 9, 21, 30, 0), categories[4].CEndDate);
//        }

//        [Test]
//        public void TestCategoriesController_AddCategory_CategoryExistButErrorIsThrow_ShouldThrowException()
//        {
//            //Arrange
//            var controller = new CategoriesController(context.Object, user.Object, mapper.Object);

//            var category = new CategoriesViewModel()
//            {
//                CID = 2,
//                CGID = new Guid("99dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//            };

//            //Act
//            //Assert
//            Assert.Throws<Exception>(() => controller.Save(category));
//        }

//        [Test]
//        public void TestCategoriesController_AddCategory_CategoryExist_ShouldModifyCategory()
//        {
//            //Arrange
//            var controller = new CategoriesController(context.Object, user.Object, mapper.Object);

//            var category = new CategoriesViewModel()
//            {
//                CID = 4,
//                CGID = new Guid("f9dacc1d-7bee-4635-9c4c-9404a4af80dd"),
//                CUID = 44,
//                CName = "Nazwa 5",
//                CBudget = 2060,
//                CStartDate = new DateTime(2023, 12, 5, 21, 30, 0),
//                CEndDate = new DateTime(2023, 12, 12, 21, 30, 0)
//            };

//            //Act
//            controller.Save(category);

//            //Assert
//            ClassicAssert.AreEqual(4, categories.Count);
//            ClassicAssert.AreEqual("Nazwa 5", categories[3].CName);
//            ClassicAssert.AreEqual(2060, categories[3].CBudget);
//            ClassicAssert.AreEqual(new DateTime(2023, 12, 5, 21, 30, 0), categories[3].CStartDate);
//            ClassicAssert.AreEqual(new DateTime(2023, 12, 12, 21, 30, 0), categories[3].CEndDate);
//        }

//        //Delete
//        [Test]
//        public void TestCategoriesController_DeleteCategory_CategoryNotFound_ShouldThrowException()
//        {
//            //Arrange
//            var controller = new CategoriesController(context.Object, user.Object, mapper.Object);

//            //Act
//            //Assert
//            Assert.Throws<Exception>(() => controller.Delete(Guid.Empty));
//        }

//        [Test]
//        public void TestCategoriesController_DeleteCategory_CategoryHasTasks_ShouldThrowException()
//        {
//            //Arrange
//            var controller = new CategoriesController(context.Object, user.Object, mapper.Object);

//            //Act
//            //Assert
//            Assert.Throws<Exception>(() => controller.Delete(Guid.Empty));
//        }

//        [Test]
//        public void TestCategoriesController_DeleteCategory_CategoryIsFound_ShouldDeleteCategory()
//        {
//            //Arrange
//            var controller = new CategoriesController(context.Object, user.Object, mapper.Object);

//            //Act
//            controller.Delete(categories[2].CGID);

//            //Assert
//            ClassicAssert.AreEqual(3, categories.Count);
//        }
//    }
//}