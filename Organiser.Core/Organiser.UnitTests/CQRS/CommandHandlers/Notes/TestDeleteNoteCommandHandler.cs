using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.CommandHandlers.Notes
{
    [TestFixture]
    public class TestDeleteNoteCommandHandler
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

            context.Setup(x => x.DeleteNote(It.IsAny<Cores.Entities.Notes>())).Callback<Cores.Entities.Notes>(note => notes.Remove(note));
        }

        [Test]
        public void TestDeleteNoteCommandHandler_DeleteNote_NoteNotFound_ShouldThrowException()
        {
            //Arrange
            var command = new DeleteNoteCommand() { NGID = new Guid() };
            var handler = new DeleteNoteCommandHandler(context.Object);

            //Act
            //Assert
            Assert.Throws<Exception>(() => handler.Handle(command));
        }

        [Test]
        public void TestDeleteNoteCommandHandler_DeleteNote_NoteWasFound_ShouldDeleteNote()
        {
            //Arrange
            var command = new DeleteNoteCommand() { NGID = notes[0].NGID };
            var handler = new DeleteNoteCommandHandler(context.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, notes.Count);

            ClassicAssert.AreEqual(new Guid("f1dacc1d-7bee-4635-9c4c-9404a4af80dd"), notes[0].NGID);
            ClassicAssert.AreEqual("Title2", notes[0].NTitle);
            ClassicAssert.AreEqual("text2", notes[0].NTxt);
        }
    }
}
