using AwsNotes.Interfaces;
using AwsNotes.models;
using AwsNotes.models.DTO;


namespace AwsNotes.Services
{
    public  class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email, 
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            //if (user == null) 
            //{
            // ошибка
            //}

            user.Username = request.Username;
            user.Email = request.Email;

            await _userRepository.UpdateAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }

    }
}
