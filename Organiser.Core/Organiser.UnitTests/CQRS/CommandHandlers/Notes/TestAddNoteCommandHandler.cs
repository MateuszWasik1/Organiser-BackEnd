using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.Exceptions.Categories;
using Organiser.Core.Exceptions.Notes;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Models.ViewModels.CategoriesViewModel;
using Organiser.Cores.Services;
using Organiser.CQRS.Resources.Categories.Commands;
using Organiser.CQRS.Resources.Categories.Handlers;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Notes
{
    [TestFixture]
    public class TestAddNoteCommandHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IUserContext>? user;

        private List<Cores.Entities.Notes>? notes;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            user = new Mock<IUserContext>();

            notes = new List<Cores.Entities.Notes>();
          
            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Notes>())).Callback<Cores.Entities.Notes>(note => notes.Add(note));

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestAddNoteCommandHandler_AddNote_TitleIsEmpty_ShouldAddThrowNoteTitleRequiredException()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NTitle = "",
                NTxt = "",
            };

            var command = new AddNoteCommand() { Model = model };
            var handler = new AddNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<NoteTitleRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddNoteCommandHandler_AddNote_TitleIsOver200_ShouldAddThrowNoteTitleMax200Exception()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NTitle = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec qua",
                NTxt = "",
            };

            var command = new AddNoteCommand() { Model = model };
            var handler = new AddNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<NoteTitleMax200Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddNoteCommandHandler_AddNote_TextIsEmpty_ShouldAddThrowNoteTextRequiredException()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NTitle = "1",
                NTxt = "",
            };

            var command = new AddNoteCommand() { Model = model };
            var handler = new AddNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<NoteTextRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddNoteCommandHandler_AddNote_TextIsOver4000_ShouldAddThrowNoteTitleMax4000Exception()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NTitle = "1",
                NTxt = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibulum volutpat pretium libero. Cras id dui. Aenean ut eros et nisl sagittis vestibulum. Nullam nulla eros, ultricies sit amet, nonummy id, imperdiet feugiat, pede. Sed lectus. Donec mollis hendrerit risus. Phasellus nec sem in justo pellentesque facilisis. Etiam imperdiet imperdiet orci. Nunc nec neque. Phasellus leo dolor, tempus non, auctor et, hendrerit quis, nisi. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo. Maecenas malesuada. Praesent congue erat at massa. Sed cursus turpis vitae tortor. Donec posuere vulputate arcu. Phasellus accumsan cursus velit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed aliquam, nisi quis porttitor congue, elit erat euismod orci, ac placerat dolor lectus quis orci. Phasellus consectetuer vestibulum elit. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Vestibulum fringilla pede sit amet augue. In turpis. Pellentesque posuere. Praesent turpis. Aenean posuere, tortor sed cursus feugiat, nunc augue blandit nunc, eu sollicitudin urna dolor sagittis lacus. Donec elit libero, sodales nec, volutpat a, suscipit non, turpis. Nullam sagittis. Suspendisse pulvinar, augue ac venenatis condimentum, sem libero volutpat nibh, nec pellentesque velit pede quis nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce id purus. Ut varius tincidunt libero. Phasellus dolor. Maecenas vestibulum mollis diam. Pellentesque ut neque. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In dui magna, posuere eget, vestibulum et, tempor auctor, justo. In ac felis quis tortor malesuada pretium. Pellentesque auctor neque nec urna. Proin sapien ipsum, porta a, auctor quis, euismod ut, mi. Aenean viverra rhoncus pede. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Ut non enim eleifend felis pretium feugiat. Vivamus quis mi. Phasellus a est. Phase",
            };

            var command = new AddNoteCommand() { Model = model };
            var handler = new AddNoteCommandHandler(context.Object, user.Object);

            //Act
            //Assert
            Assert.Throws<NoteTitleMax4000Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestAddNoteCommandHandler_AddNote_ShouldAddNote()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NTitle = "New Title",
                NTxt = "New text",
            };

            var command = new AddNoteCommand() { Model = model };
            var handler = new AddNoteCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, notes.Count);
            ClassicAssert.AreEqual(model.NTitle, notes[0].NTitle);
            ClassicAssert.AreEqual(model.NTxt, notes[0].NTxt);
        }
    }
}
