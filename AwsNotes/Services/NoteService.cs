using AwsNotes.Interfaces;
using AwsNotes.models;
using AwsNotes.models.DTO;
using AwsNotes.Repos;

namespace AwsNotes.Services
{
    public  class NoteService : INoteService
    {

        private readonly NoteRepository _noteRepository;
        private readonly IRepository<User> _userRepository;

        public NoteService(NoteRepository noteRepository, IRepository<User> userRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }

        public async Task<Note?> GetNoteByIdAsync(string id)
        {
            return await _noteRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _noteRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Note>> GetNotesByUserIdAsync(string userId)
        {
            return await _noteRepository.GetByUserIdAsync(userId);
        }

        public async Task<Note> CreateNoteAsync(CreateNoteRequest request)
        {

            var user = await _userRepository.GetByIdAsync(request.UserId);

            //if (user == null)
            //{
            //    throw new KeyNotFoundException($"User with ID {request.UserId} not found");
            //}

            var note = new Note
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content
            };

            return await _noteRepository.CreateAsync(note);
        }

        public async Task<Note> UpdateNoteAsync(UpdateNoteRequest request)
        {
            var note = await _noteRepository.GetByIdAsync(request.Id);

            //if (note == null)
            //{
            //    throw new KeyNotFoundException($"Note with ID {id} not found");
            //}

            note.Title = request.Title;
            note.Content = request.Content;

            await _noteRepository.UpdateAsync(note);
            return note;
        }

        public async Task DeleteNoteAsync(string id)
        {
            await _noteRepository.DeleteAsync(id);
        }
    }
}
