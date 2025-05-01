namespace AwsNotes.models.DTO
{
    public class CreateNoteRequest
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
