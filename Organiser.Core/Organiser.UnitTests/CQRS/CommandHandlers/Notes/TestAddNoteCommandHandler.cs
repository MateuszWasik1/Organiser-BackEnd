using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Commands;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;
using Organiser.Cores.Services;

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

            //context.Setup(x => x.CreateOrUpdate(It.IsAny<Cores.Entities.Notes>())).Callback<Cores.Entities.Notes>(note =>
            //{
            //    var currentCategory = categories.FirstOrDefault(x => x.CID == category.CID);

            //    if (currentCategory != null)
            //        categories[categories.FindIndex(x => x.CID == currentCategory.CID)] = category;
            //    else
            //        categories.Add(category);
            //});

            user.Setup(x => x.UID).Returns(1);
        }

        [Test]
        public void TestAddNoteCommandHandler_AddNote_ShouldAddNote()
        {
            //Arrange
            var model = new NotesAddViewModel()
            {
                NGID = new Guid(),
                NTitle = "New Title",
                NTxt = "New text",
            };

            var command = new AddNoteCommand() { Model = model };
            var handler = new AddNoteCommandHandler(context.Object, user.Object);

            //Act
            handler.Handle(command);

            //Assert
            ClassicAssert.AreEqual(1, notes.Count);
            ClassicAssert.AreEqual(model.NGID, notes[0].NGID);
            ClassicAssert.AreEqual(model.NTitle, notes[0].NTitle);
            ClassicAssert.AreEqual(model.NTxt, notes[0].NTxt);
        }
    }
}
