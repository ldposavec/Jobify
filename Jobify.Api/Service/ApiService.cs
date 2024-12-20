using Jobify.Api.DTOs;
using Jobify.BL.DALModels;
using System.Net.Http;

namespace Jobify.Api.Service
{
    public class ApiService
    {
        private const string CLIENT = "Jobify.Api";
        private readonly IHttpClientFactory _clientFactory;

        public ApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> RegisterEmployerAsync(EmployerRegistrationDTO dto)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.Employer.Register, dto);
            return response; 
        }

        public async Task<HttpResponseMessage> VerifyEmailAsync(string token)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.GetAsync($"api/Employer/verify-email?token={token}");
            return response;
        }        
        
        public async Task<HttpResponseMessage> GetJwtTokenAsync(string email)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.Employer.JWT, email);
            return response;
        }

        public async Task<HttpResponseMessage> RegisterStudentAsync(StudentRegistrationDTO dto)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.Student.Register, dto);
            return response;
        }

        public async Task<List<UserTypeDTO>> GetUserTypes()
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.GetAsync(ApiRoutes.UserType.Base);
            if (response.IsSuccessStatusCode)
            {
                var userTypes = await response.Content.ReadFromJsonAsync<List<UserTypeDTO>>();
                return userTypes ?? new List<UserTypeDTO>();
            }
            throw new Exception("Error fetching user types.");
        }        
        
        public async Task<HttpResponseMessage> LoginAsync(LoginDTO dto)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.User.Login, dto);
            return response;
        }        
        
        public async Task<HttpResponseMessage> ChangePasswordAsync(UserChangePasswordDto dto)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.User.ChangePassword, dto);
            return response;
        }
        public async Task<HttpResponseMessage> SendPasswordResetEmailAsync(string email)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var requestUri = $"{ApiRoutes.User.SendPasswordResetEmail}?email={Uri.EscapeDataString(email)}";
            var response = await client.PostAsync(requestUri, null); 
            return response;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.GetAsync(ApiRoutes.User.Base);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                return users ?? new List<UserDTO>();
            }
            throw new Exception("Error fetching users.");
        }

        public async Task<bool> DeleteUser(int id)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.DeleteAsync($"{ApiRoutes.User.Base}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;  
            }

            throw new Exception("Error deleting user.");
        }

        public async Task<HttpResponseMessage> CreateUser(UserDTO user)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.User.Base, user);
            return response;
        }

        public async Task<bool> UpdateUser(int userId, UserDTO user)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PutAsJsonAsync($"{ApiRoutes.User.Base}/{userId}", user);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new Exception("Error updating user.");
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.GetAsync($"{ApiRoutes.User.Base}/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                return user ?? new UserDTO();
            }
            
            throw new Exception($"Failed to fetch user with ID {userId}.");
        }

    }
}
