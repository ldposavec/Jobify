using Jobify.Api.DTOs;
using Jobify.BL.DALModels;
using System.Net.Http;

namespace Jobify.Api.Service
{
    public class ApiService : IApiService
    {
        private const string CLIENT = "Jobify.Api";
        private readonly HttpClient _client;

        private readonly Dictionary<Type, string> _baseRoutes = new()
        {
            { typeof(UserDTO), ApiRoutes.User.Base }, { typeof(FirmDTO), ApiRoutes.Firm.Base },
            { typeof(FirmSimplifiedDTO), ApiRoutes.Firm.Base }, { typeof(ReviewDTO), ApiRoutes.Review.Base }
        };
        public ApiService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient(CLIENT);
        }
        private string GetRouteForType<T>()
        {
            if (_baseRoutes.TryGetValue(typeof(T), out var route))
            {
                return route;
            }
            throw new Exception($"Route not defined for type {typeof(T).Name}");
        }
        public async Task<List<UserTypeDTO>> GetUserTypes()
        {
            var response = await _client.GetAsync(ApiRoutes.UserType.Base);
            if (response.IsSuccessStatusCode)
            {
                var userTypes = await response.Content.ReadFromJsonAsync<List<UserTypeDTO>>();
                return userTypes ?? new List<UserTypeDTO>();
            }
            throw new Exception("Error fetching user types.");
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            var route = GetRouteForType<T>();
            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<T>>();
                return data ?? new List<T>();
            }
            throw new Exception($"Error fetching data from {route}");
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var route = $"{GetRouteForType<T>()}/{id}";
            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return data ?? Activator.CreateInstance<T>();
            }
            throw new Exception($"Error fetching data from {route}");
        }

        public async Task<HttpResponseMessage> CreateAsync<T>(T entity)
        {
            var route = GetRouteForType<T>();
            return await _client.PostAsJsonAsync(route, entity);
        }

        public async Task<bool> UpdateAsync<T>(int id, T entity)
        {
            var route = $"{GetRouteForType<T>()}/{id}";
            var response = await _client.PutAsJsonAsync(route, entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync<T>(int id)
        {
            var route = $"{GetRouteForType<T>()}/{id}";
            var response = await _client.DeleteAsync(route);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<T>> GetItemsByFirmIdAsync<T>(int firmId)
        {
            var route = $"{GetRouteForType<T>()}/firm/{firmId}";
            var response = await _client.GetAsync(route);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<T>>();
                return data ?? new List<T>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<T>();
            }
            throw new Exception($"Error fetching for patient ID {firmId} from {route}");
        }
    }
}
