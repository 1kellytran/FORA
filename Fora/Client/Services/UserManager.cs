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
        public async Task<UserModel> GetUserById(int id)
        {
            UserModel user = await _httpClient.GetFromJsonAsync<UserModel>($"api/user/{id}");
            return user;
        }

        public async Task<string> SignUpUser(UserDTOModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", user);

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }

        public async Task<string> SignInUser(UserDTOModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/signin", user);
            throw new NotImplementedException();
        }

       
        public async Task DeleteUser(int id)
        {
            await _httpClient.DeleteAsync($"api/user/{id}");
            
        }
    }
}
