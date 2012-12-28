using System.Collections.Generic;

using TripPoint.Model.Domain;

namespace TripPoint.Model.Data.Repository
{
    public interface INoteRepository
    {
        void DeleteNotes(IEnumerable<Note> notes);
    }
}
