using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using AwsNotes.models;
using AwsNotes.models.DTO;
using AwsNotes.Repos;
using AwsNotes.Services;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsNotes;

public class AppSyncRequest
{
    public string Field { get; set; } 
    public IDictionary<string, object> Arguments { get; set; } 
}


public class Function
{
    private readonly NoteService _noteService;
    private readonly UserService _userService;

    public Function()
    {
        // Set up DynamoDB client and context
        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDbContext = new DynamoDBContext(dynamoDbClient);

        // Create repositories
        var noteRepository = new NoteRepository(dynamoDbContext);
        var userRepository = new UserRepository(dynamoDbContext);

        // Initialize services
        _noteService = new NoteService(noteRepository, userRepository);
        _userService = new UserService(userRepository);
    }

    // Универсальный обработчик для вызова разных методов
    public async Task<object> FunctionHandler(AppSyncRequest input, ILambdaContext context)
    {

        context.Logger.LogInformation($"Received request with Field: {input?.Field ?? "null"}");
        context.Logger.LogInformation($"Arguments: {JsonSerializer.Serialize(input?.Arguments ?? new Dictionary<string, object>())}");

        switch (input.Field)
        {
            case "GetUser":
                return await GetUser(input.Arguments["Id"].ToString(), context);

            case "CreateUser":
                return await CreateUser(new CreateUserRequest
                {
                    Username = input.Arguments["Username"].ToString(),
                    Email = input.Arguments["Email"].ToString()
                }, context);

            case "UpdateUser":
                return await UpdateUser(new UpdateUserRequest
                {
                    Id = input.Arguments["Id"].ToString(),
                    Username = input.Arguments["Username"].ToString(),
                    Email = input.Arguments["Email"].ToString()
                }, context);

            case "DeleteUser":
                return await DeleteUser(input.Arguments["Id"].ToString(), context);

            case "GetNote":
                return await GetNote(input.Arguments["Id"].ToString(), context);

            case "CreateNote":
                return await CreateNote(new CreateNoteRequest
                {
                    UserId = input.Arguments["UserId"].ToString(),
                    Title = input.Arguments["Title"].ToString(),
                    Content = input.Arguments["Content"].ToString()
                }, context);

            case "UpdateNote":
                return await UpdateNote(new UpdateNoteRequest
                {
                    Id = input.Arguments["Id"].ToString(),
                    Title = input.Arguments["Title"].ToString(),
                    Content = input.Arguments["Content"].ToString()
                }, context);

            case "DeleteNote":
                return await DeleteNote(input.Arguments["Id"].ToString(), context);

            case "GetAllUsers":
                return await GetAllUsers(context);

            case "GetAllNotes":
                return await GetAllNotes(context);

            case "GetNotesByUserId":
                return await GetNotesByUserId(input.Arguments["UserId"].ToString(), context);
        }

        return new ApiResponse<string>
        {
            Success = false,
            Message = $"Операция '{input.Field}' не поддерживается."
        };
    }

    // User CRUD functions
    public async Task<ApiResponse<User>> GetUser(string id, ILambdaContext context)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = $"User with ID {id} not found"
                };
            }

            return new ApiResponse<User>
            {
                Success = true,
                Data = user
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error getting user: {ex.Message}");
            return new ApiResponse<User>
            {
                Success = false,
                Message = $"Error retrieving user: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<User>>> GetAllUsers(ILambdaContext context)
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return new ApiResponse<IEnumerable<User>>
            {
                Success = true,
                Data = users
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error getting all users: {ex.Message}");
            return new ApiResponse<IEnumerable<User>>
            {
                Success = false,
                Message = $"Error retrieving users: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<User>> CreateUser(CreateUserRequest request, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Creating user: {request.Username}");
            var user = await _userService.CreateUserAsync(request);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User created successfully",
                Data = user
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error creating user: {ex.Message}");
            return new ApiResponse<User>
            {
                Success = false,
                Message = $"Error creating user: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<User>> UpdateUser(UpdateUserRequest input, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Updating user with ID: {input.Id}");

            var user = await _userService.UpdateUserAsync(input);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User updated successfully",
                Data = user
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error updating user: {ex.Message}");
            return new ApiResponse<User>
            {
                Success = false,
                Message = $"Error updating user: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteUser(string id, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Deleting user with ID: {id}");
            await _userService.DeleteUserAsync(id);

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "User deleted successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error deleting user: {ex.Message}");
            return new ApiResponse<bool>
            {
                Success = false,
                Message = $"Error deleting user: {ex.Message}"
            };
        }
    }

    // User Note functions
    public async Task<ApiResponse<Note>> GetNote(string id, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Getting note with ID: {id}");
            var note = await _noteService.GetNoteByIdAsync(id);

            if (note == null)
            {
                return new ApiResponse<Note>
                {
                    Success = false,
                    Message = $"Note with ID {id} not found"
                };
            }

            return new ApiResponse<Note>
            {
                Success = true,
                Data = note
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error getting note: {ex.Message}");
            return new ApiResponse<Note>
            {
                Success = false,
                Message = $"Error retrieving note: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<Note>>> GetAllNotes(ILambdaContext context)
    {
        try
        {
            var notes = await _noteService.GetAllNotesAsync();
            return new ApiResponse<IEnumerable<Note>>
            {
                Success = true,
                Data = notes
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error getting all notes: {ex.Message}");
            return new ApiResponse<IEnumerable<Note>>
            {
                Success = false,
                Message = $"Error retrieving notes: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<Note>>> GetNotesByUserId(string userId, ILambdaContext context)
    {
        try
        {
            var notes = await _noteService.GetNotesByUserIdAsync(userId);
            return new ApiResponse<IEnumerable<Note>>
            {
                Success = true,
                Data = notes
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error getting notes for user {userId}: {ex.Message}");
            return new ApiResponse<IEnumerable<Note>>
            {
                Success = false,
                Message = $"Error retrieving notes: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<Note>> CreateNote(CreateNoteRequest request, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Creating note for user: {request.UserId}");
            var note = await _noteService.CreateNoteAsync(request);

            return new ApiResponse<Note>
            {
                Success = true,
                Message = "Note created successfully",
                Data = note
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error creating note: {ex.Message}");
            return new ApiResponse<Note>
            {
                Success = false,
                Message = $"Error creating note: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<Note>> UpdateNote(UpdateNoteRequest input, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Updating note with ID: {input.Id}");

            var note = await _noteService.UpdateNoteAsync(input);

            return new ApiResponse<Note>
            {
                Success = true,
                Message = "Note updated successfully",
                Data = note
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error updating note: {ex.Message}");
            return new ApiResponse<Note>
            {
                Success = false,
                Message = $"Error updating note: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteNote(string id, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation($"Deleting note with ID: {id}");
            await _noteService.DeleteNoteAsync(id);

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Note deleted successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error deleting note: {ex.Message}");
            return new ApiResponse<bool>
            {
                Success = false,
                Message = $"Error deleting note: {ex.Message}"
            };
        }
    }
}

