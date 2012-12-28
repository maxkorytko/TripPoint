using System.Linq;
using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public class DatabaseNoteRepository : DatabaseRepository, INoteRepository
    {
        public DatabaseNoteRepository(TripPointDataContext dataContext)
            : base(dataContext)
        {
        }

        public void DeleteNotes(IEnumerable<Note> notes)
        {
            if (notes == null) return;

            foreach (var note in notes)
            {
                var noteEntity = DataContext.Notes.SingleOrDefault(n => n.ID == note.ID);

                if (noteEntity == null) continue;

                // we need to remove the note from the Checkpoint notes collection
                // to trigger data binding
                //
                note.Checkpoint.Notes.Remove(note);
                DataContext.Notes.DeleteOnSubmit(noteEntity);
            }

            DataContext.SubmitChanges();
        }
    }
}
