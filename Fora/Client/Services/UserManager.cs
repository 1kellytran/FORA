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

        //public async Task AddUser(UserModel user)
        //{
        //    await _httpClient.PostAsJsonAsyn("api/user", user);
        //}
    }
}
