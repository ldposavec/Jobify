using Jobify.Api.DTOs;

namespace Jobify.Api.Service
{
    public interface IAccountApiService
    {
        Task<HttpResponseMessage> RegisterEmployerAsync(EmployerRegistrationDTO dto);
        Task<HttpResponseMessage> VerifyEmailAsync(string token);
        Task<HttpResponseMessage> GetJwtTokenAsync(string email);
        Task<HttpResponseMessage> RegisterStudentAsync(StudentRegistrationDTO dto);
        Task<(string Token, string Role)> LoginAsync(LoginDTO dto);
        Task<HttpResponseMessage> ChangePasswordAsync(UserChangePasswordDto dto);
        Task<HttpResponseMessage> SendPasswordResetEmailAsync(string email);
        Task<int> GetUserIdByEmailAsync(string email);
    }
}
