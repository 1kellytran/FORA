using Fora.Shared;
using System.Net.Http.Json;

namespace Fora.Client.Services
{
    public class DataManager : IDataManager
    {
        private readonly HttpClient _httpClient;

        public DataManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<InterestModel>> GetAllUsers()
        {
            
            var result = await _httpClient.GetFromJsonAsync<List<InterestModel>>("api/interest"); 
            return result;

        }
    }
}
