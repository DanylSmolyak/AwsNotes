using Amazon.DynamoDBv2.DataModel;
using AwsNotes.Interfaces;
using AwsNotes.models;

namespace AwsNotes.Repos
{
    public class UserRepository : IRepository<User>
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public UserRepository(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _dynamoDbContext.LoadAsync<User>(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var scanConditions = new List<ScanCondition>();
            return await _dynamoDbContext.ScanAsync<User>(scanConditions).GetRemainingAsync();
        }

        public async Task<User> CreateAsync(User entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _dynamoDbContext.SaveAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(User entity)
        {
            var existingUser = await GetByIdAsync(entity.Id);
            if (existingUser == null)
            {
                throw new Exception($"User with ID {entity.Id} not found.");
            }

            entity.CreatedAt = existingUser.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;

            await _dynamoDbContext.SaveAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _dynamoDbContext.DeleteAsync<User>(id);
        }
    }
}