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

        public async Task RegisterEmployerAsync(EmployerRegistrationDTO dto)
        {
            var client = _clientFactory.CreateClient(CLIENT);
            var response = await client.PostAsJsonAsync(ApiRoutes.Employer.Register, dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
