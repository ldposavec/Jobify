using Jobify.Api.DTOs;

namespace Jobify.Api.Service
{
    public class AccountApiService : IAccountApiService
    {
        private const string CLIENT = "Jobify.Api";
        private readonly HttpClient _client;
        public AccountApiService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(CLIENT);
        }
        public async Task<HttpResponseMessage> RegisterEmployerAsync(EmployerRegistrationDTO dto)
        {
            var response = await _client.PostAsJsonAsync(ApiRoutes.Employer.Register, dto);
            return response;
        }

        public async Task<HttpResponseMessage> VerifyEmailAsync(string token)
        {
            var response = await _client.GetAsync($"api/Employer/verify-email?token={token}");
            return response;
        }

        public async Task<HttpResponseMessage> GetJwtTokenAsync(string email)
        {
            var response = await _client.PostAsJsonAsync(ApiRoutes.Employer.JWT, email);
            return response;
        }

        public async Task<HttpResponseMessage> RegisterStudentAsync(StudentRegistrationDTO dto)
        {
            var response = await _client.PostAsJsonAsync(ApiRoutes.Student.Register, dto);
            return response;
        }
        public async Task<(string Token, string Role)> LoginAsync(LoginDTO dto)
        {
            var response = await _client.PostAsJsonAsync(ApiRoutes.User.Login, dto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return (result.Token, result.Role);
            }
            throw new Exception("Invalid login attempt");
        }

        public async Task<HttpResponseMessage> ChangePasswordAsync(UserChangePasswordDto dto)
        {
            var response = await _client.PostAsJsonAsync(ApiRoutes.User.ChangePassword, dto);
            return response;
        }
        public async Task<HttpResponseMessage> SendPasswordResetEmailAsync(string email)
        {
            var requestUri = $"{ApiRoutes.User.SendPasswordResetEmail}?email={Uri.EscapeDataString(email)}";
            var response = await _client.PostAsync(requestUri, null);
            return response;
        }

        public async Task<int> GetUserIdByEmailAsync(string email)
        {
            var requestUri = $"{ApiRoutes.User.UserByMail}?email={Uri.EscapeDataString(email)}";
            var response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var userId = await response.Content.ReadAsStringAsync();
                return int.Parse(userId);
            }
            throw new Exception($"Unable to retrieve user ID for email {email}.");
        }

    }
}
