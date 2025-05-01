using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AwsNotes.Interfaces;
using AwsNotes.models;

namespace AwsNotes.Repos
{
    public class NoteRepository : IRepository<Note>
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public NoteRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<Note> GetByIdAsync(string id)
        {
            return await _dynamoDbContext.LoadAsync<Note>(id);
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            var scanConditions = new List<ScanCondition>();
            return await _dynamoDbContext.ScanAsync<Note>(scanConditions).GetRemainingAsync();
        }

        public async Task<Note> CreateAsync(Note entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _dynamoDbContext.SaveAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(Note entity)
        {
            var existingNote = await GetByIdAsync(entity.Id);
            if (existingNote == null)
            {
                throw new Exception($"Note with ID {entity.Id} not found.");
            }

            entity.UserId = existingNote.UserId;
            entity.CreatedAt = existingNote.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;

            await _dynamoDbContext.SaveAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _dynamoDbContext.DeleteAsync<Note>(id);
        }

        public async Task<IEnumerable<Note>> GetByUserIdAsync(string userId)
        {
            var queryConfig = new QueryOperationConfig
            {
                IndexName = "UserIdIndex",
                KeyExpression = new Expression
                {
                    ExpressionStatement = "user_id = :userId",
                    ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                    {
                        { ":userId", userId }
                    }
                }
            };

            return await _dynamoDbContext.FromQueryAsync<Note>(queryConfig).GetRemainingAsync();
        }
    }
}