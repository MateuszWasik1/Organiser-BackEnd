using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.Exceptions.Notes;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Entities;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Notes
{
    [TestFixture]
    public class TestUpdateNoteCommandHandler
    {
        private Mock<IDataBaseContext>? context;

        private List<Cores.Entities.Notes>? notes;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();

            notes = new List<Cores.Entities.Notes>()
            {
                new Cores.Entities.Notes()
                {
                    NID = 1,
                    NGID = new Guid("f0dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NTitle = "Title1",
                    NTxt = "text1",
                },
                new Cores.Entities.Notes()
                {
                    NID = 2,
                    NGID = new Guid("f1dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NTitle = "Title2",
                    NTxt = "text2",
                },
            };
            
            context.Setup(x => x.Notes).Returns(notes.AsQueryable());

            context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Notes>())).Callback<Cores.Entities.Notes>(note => notes[notes.FindIndex(x => x.NID == note.NID)] = note);
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_TitleIsEmpty_ShouldAddThrowNoteTitleRequiredException()
        {
            //Arrange
            var model = new NotesUpdateViewModel()
            {
                NGID = notes[0].NGID,
                NTitle = "",
                NTxt = "",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<NoteTitleRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_TitleIsOver200_ShouldAddThrowNoteTitleMax200Exception()
        {
            //Arrange
            var model = new NotesUpdateViewModel()
            {
                NGID = notes[0].NGID,
                NTitle = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec qua",
                NTxt = "",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<NoteTitleMax200Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_TextIsEmpty_ShouldAddThrowNoteTextRequiredException()
        {
            //Arrange
            var model = new NotesUpdateViewModel()
            {
                NGID = notes[0].NGID,
                NTitle = "1",
                NTxt = "",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<NoteTextRequiredException>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_TextIsOver4000_ShouldAddThrowNoteTitleMax4000Exception()
        {
            //Arrange
            var model = new NotesUpdateViewModel()
            {
                NGID = notes[0].NGID,
                NTitle = "1",
                NTxt = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibulum volutpat pretium libero. Cras id dui. Aenean ut eros et nisl sagittis vestibulum. Nullam nulla eros, ultricies sit amet, nonummy id, imperdiet feugiat, pede. Sed lectus. Donec mollis hendrerit risus. Phasellus nec sem in justo pellentesque facilisis. Etiam imperdiet imperdiet orci. Nunc nec neque. Phasellus leo dolor, tempus non, auctor et, hendrerit quis, nisi. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo. Maecenas malesuada. Praesent congue erat at massa. Sed cursus turpis vitae tortor. Donec posuere vulputate arcu. Phasellus accumsan cursus velit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed aliquam, nisi quis porttitor congue, elit erat euismod orci, ac placerat dolor lectus quis orci. Phasellus consectetuer vestibulum elit. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Vestibulum fringilla pede sit amet augue. In turpis. Pellentesque posuere. Praesent turpis. Aenean posuere, tortor sed cursus feugiat, nunc augue blandit nunc, eu sollicitudin urna dolor sagittis lacus. Donec elit libero, sodales nec, volutpat a, suscipit non, turpis. Nullam sagittis. Suspendisse pulvinar, augue ac venenatis condimentum, sem libero volutpat nibh, nec pellentesque velit pede quis nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce id purus. Ut varius tincidunt libero. Phasellus dolor. Maecenas vestibulum mollis diam. Pellentesque ut neque. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In dui magna, posuere eget, vestibulum et, tempor auctor, justo. In ac felis quis tortor malesuada pretium. Pellentesque auctor neque nec urna. Proin sapien ipsum, porta a, auctor quis, euismod ut, mi. Aenean viverra rhoncus pede. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Ut non enim eleifend felis pretium feugiat. Vivamus quis mi. Phasellus a est. Phase",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<NoteTitleMax4000Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_NoteNotFound_ShouldThrowNoteNotFoundException()
        {
            //Arrange
            var model = new NotesUpdateViewModel()
            {
                NGID = new Guid(),
                NTitle = "New Title",
                NTxt = "New text",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<NoteNotFoundException>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_NoteWasFound_ShouldUpdateNote()
        {
            //Arrange
            var model = new NotesUpdateViewModel()
            {
                NGID = notes[0].NGID,
                NTitle = "New Title",
                NTxt = "New text",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(2, notes.Count);

            ClassicAssert.AreEqual(model.NGID, notes[0].NGID);
            ClassicAssert.AreEqual(model.NTitle, notes[0].NTitle);
            ClassicAssert.AreEqual(model.NTxt, notes[0].NTxt);

            ClassicAssert.AreEqual(new Guid("f1dacc1d-7bee-4635-9c4c-9404a4af80dd"), notes[1].NGID);
            ClassicAssert.AreEqual("Title2", notes[1].NTitle);
            ClassicAssert.AreEqual("text2", notes[1].NTxt);
        }
    }
}
