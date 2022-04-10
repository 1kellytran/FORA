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

        public async Task CreateInterest(InterestModel interestToAdd)
        {
            var response = await _httpClient.PostAsJsonAsync("api/interest", interestToAdd);            
        }
        public async Task<List<InterestModel>> GetAllInterests()
        {
            List<InterestModel> interests = new();
            interests = await _httpClient.GetFromJsonAsync<List<InterestModel>>("api/interest");
            
             return interests;            
        }

        public async Task<string> DeleteInterest(int id)
        {
            var result = await _httpClient.DeleteAsync("api/intesrest");
            return result.ToString(); //är detta strängen message?
        }


    }
}
