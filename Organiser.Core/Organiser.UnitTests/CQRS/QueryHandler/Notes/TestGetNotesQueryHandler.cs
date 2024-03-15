using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Organiser.Core.CQRS.Resources.Notes.Handlers;
using Organiser.Core.CQRS.Resources.Notes.Queries;
using Organiser.Core.Models.ViewModels.NotesViewModels;
using Organiser.Cores.Context;

namespace Organiser.UnitTests.CQRS.QueryHandler.Notes
{
    [TestFixture]
    public class TestGetNotesQueryHandler
    {
        private Mock<IDataBaseContext>? context;
        private Mock<IMapper>? mapper;

        private List<Cores.Entities.Notes>? notes;
        private List<NotesViewModel>? notesViewModel;

        [SetUp]
        public void SetUp()
        {
            context = new Mock<IDataBaseContext>();
            mapper = new Mock<IMapper>();

            notes = new List<Cores.Entities.Notes>() 
            {
                new Cores.Entities.Notes()
                {
                    NID = 1,
                    NGID = new Guid("f3dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NUID = 1,
                    NDate = new DateTime(2024, 3, 16, 16, 45, 0),
                    NModificationDate = new DateTime(2024, 3, 16, 16, 50, 0),
                    NTitle = "Old Title",
                    NTxt = "Old text",
                },
                new Cores.Entities.Notes()
                {
                    NID = 2,
                    NGID = new Guid("f4dacc1d-7bee-4635-9c4c-9404a4af80dd"),
                    NUID = 2,
                    NDate = new DateTime(2024, 3, 17, 16, 45, 0),
                    NModificationDate = new DateTime(2024, 3, 17, 16, 50, 0),
                    NTitle = "Old Title2",
                    NTxt = "Old text2",
                }
            };

            notesViewModel = new List<NotesViewModel>();

            context.Setup(x => x.Notes).Returns(notes.AsQueryable());

            mapper.Setup(m => m.Map<Cores.Entities.Notes, NotesViewModel>(It.IsAny<Cores.Entities.Notes>()))
                .Callback<Cores.Entities.Notes>((Cores.Entities.Notes note) =>
                    notesViewModel.Add(
                        new NotesViewModel()
                        {
                            NGID = note.NGID,
                            NDate = note.NDate,
                            NModificationDate = note.NModificationDate,
                            NTitle = note.NTitle,
                            NTxt = note.NTxt,
                        }
                    )
                );
        }

        //Get
        [Test]
        public void TestGetNotesQueryHandler_GetAllDataForUser_ShouldReturn2Notes()
        {
            //Arrange
            var query = new GetNotesQuery();
            var handler = new GetNotesQueryHandler(context.Object, mapper.Object);

            //Act
            var result = handler.Handle(query);

            //Assert
            ClassicAssert.AreEqual(notes[0].NGID, notesViewModel[0].NGID);
            ClassicAssert.AreEqual(notes[0].NTitle, notesViewModel[0].NTitle);
            ClassicAssert.AreEqual(notes[0].NTxt, notesViewModel[0].NTxt);

            ClassicAssert.AreEqual(notes[1].NGID, notesViewModel[1].NGID);
            ClassicAssert.AreEqual(notes[1].NTitle, notesViewModel[1].NTitle);
            ClassicAssert.AreEqual(notes[1].NTxt, notesViewModel[1].NTxt);
        }
    }
}
