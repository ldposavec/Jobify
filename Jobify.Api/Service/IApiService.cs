using Jobify.Api.DTOs;

namespace Jobify.Api.Service
{
    public interface IApiService
    {
        Task<List<UserTypeDTO>> GetUserTypes();
        Task<List<T>> GetAllAsync<T>();
        Task<T> GetByIdAsync<T>(int id);
        Task<HttpResponseMessage> CreateAsync<T>(T entity);
        Task<bool> UpdateAsync<T>(int id, T entity);
        Task<bool> DeleteAsync<T>(int id);
        Task<List<T>> GetItemsByFirmIdAsync<T>(int firmId);
    }
}
