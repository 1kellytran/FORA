
using Blazored.LocalStorage;
using Fora.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;



namespace Fora.Client.Services
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserManager(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
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


            if (list != null)
            {
                await _localStorage.SetItemAsync("Token", list[0]);
                await _localStorage.SetItemAsync("Name", list[1]);
                return list;
            }

            return null;


        }

        //public async Task SignOutUser()
        //{

        //}

        public async Task DeleteUser(int id)
        {
            await _httpClient.DeleteAsync($"api/user/{id}");            
        }

        public async Task<UserStatusDTOModel> CheckUserLogin(string token)
        {
            var response = await _httpClient.GetAsync($"api/user/check?accessToken={token}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserStatusDTOModel>(result);
                return data;
            }
            return null;
            

        }
    }
}
