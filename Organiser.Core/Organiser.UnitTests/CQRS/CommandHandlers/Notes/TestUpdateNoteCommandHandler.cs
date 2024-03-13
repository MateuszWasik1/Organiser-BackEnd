using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;

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
        public void TestUpdateNoteCommandHandler_UpdateNote_NoteNotFound_ShouldThrowException()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NGID = new Guid(),
                NTitle = "New Title",
                NTxt = "New text",
            };

            var command = new UpdateNoteCommand() { Model = model };
            var handler = new UpdateNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestUpdateNoteCommandHandler_UpdateNote_NoteWasFound_ShouldUpdateNote()
        {
            //Arrange
            var model = new NotesAddViewModel()
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
