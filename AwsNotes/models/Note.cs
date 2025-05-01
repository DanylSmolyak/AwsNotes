using Amazon.DynamoDBv2.DataModel;

namespace AwsNotes.models
{
    [DynamoDBTable("Note")]
    public class Note
    {
        [DynamoDBHashKey("Id")]
        public string Id { get; set; }

        [DynamoDBProperty("user_id")]
        public string UserId { get; set; }

        [DynamoDBProperty("title")]
        public string Title { get; set; }

        [DynamoDBProperty("content")]
        public string Content { get; set; }

        [DynamoDBProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
