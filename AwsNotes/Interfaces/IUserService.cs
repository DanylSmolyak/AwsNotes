using AwsNotes.models.DTO;
using AwsNotes.models;

namespace AwsNotes.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User> UpdateUserAsync(UpdateUserRequest request);
        Task DeleteUserAsync(string id);
    }
}
