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
        
        public async Task<UserModel> GetUserToken(string token)
        {
            UserModel user = await _httpClient.GetFromJsonAsync<UserModel>($"api/user/{token}");
            return user;
        }

        public async Task<List<string>> SignUpUser(UserDTOModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", user);
            var list = await response.Content.ReadFromJsonAsync<List<string>>();

            return list;
        }

        public async Task<List<string>> SignInUser(SignInModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/signin", user);
     
            var list = await response.Content.ReadFromJsonAsync<List<string>>();

            return list;
        }

        public async Task SignOutUser()
        {

        }

        public async Task DeleteUser(int id)
        {
            await _httpClient.DeleteAsync($"api/user/{id}");            
        }
    }
}
