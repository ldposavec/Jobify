using Jobify.Api.DTOs;

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

        public async Task<HttpResponseMessage> GetUserTypes()
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.GetAsync(ApiRoutes.UserType.Values);
            return response;
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
    }
}
