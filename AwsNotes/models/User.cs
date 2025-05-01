using Amazon.DynamoDBv2.DataModel;

namespace AwsNotes.models
{
    [DynamoDBTable("User")]
    public class User
    {
        [DynamoDBHashKey("Id")]
        public string Id { get; set; }

        [DynamoDBProperty("username")]
        public string Username { get; set; }

        [DynamoDBProperty("email")]
        public string Email { get; set; }

        [DynamoDBProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
