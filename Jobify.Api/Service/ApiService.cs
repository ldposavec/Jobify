﻿using Jobify.Api.DTOs;

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

    }
}
