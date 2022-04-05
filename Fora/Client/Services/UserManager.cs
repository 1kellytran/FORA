using Fora.Shared;
using System.Net.Http.Json;

namespace Fora.Client.Services
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserModel> GetUser(int id)
        {
            UserModel user = await _httpClient.GetFromJsonAsync<UserModel>($"api/user/{id}");
            return user;
        }

        public async Task<string> AddUser(UserDTOModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", user);

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }

       
    }
}
