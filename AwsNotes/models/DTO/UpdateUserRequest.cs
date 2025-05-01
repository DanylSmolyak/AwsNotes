namespace AwsNotes.models.DTO
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

}
