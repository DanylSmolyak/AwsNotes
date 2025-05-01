using AwsNotes.models.DTO;
using AwsNotes.models;


namespace AwsNotes.Interfaces
{
    public interface INoteService
    {
        Task<Note?> GetNoteByIdAsync(string id);
        Task<Note> CreateNoteAsync(CreateNoteRequest request);
        Task<Note> UpdateNoteAsync(UpdateNoteRequest request);
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task DeleteNoteAsync(string id);
    }
}
