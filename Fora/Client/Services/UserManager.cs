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
        
        public async Task<UserModel> GetUserByToken(string accessToken)
        {
            var response = await _httpClient.GetAsync($"api/user/getbytoken?accessToken={accessToken}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var userFromDb = JsonConvert.DeserializeObject<UserModel>(result);
                return userFromDb;
            }
            return null;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync($"api/user/gau");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var userFromDb = JsonConvert.DeserializeObject<List<UserModel>>(result);
                return userFromDb;
            }
            return null;
        }

        public async Task<List<string>> SignUpUser(UserDTOModel user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", user);
            var list = await response.Content.ReadFromJsonAsync<List<string>>();

            if (list != null)
            {
                await _localStorage.SetItemAsync("Token", list[0]);
                await _localStorage.SetItemAsync("Name", list[1]);
                return list;
            }
            return null;
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

        public async Task SignOutUser()
        {
            await _localStorage.RemoveItemAsync("Token");
            await _localStorage.RemoveItemAsync("Name");
        }

        public async Task DeleteUser(int userId, string userToken)
        {
            await _httpClient.DeleteAsync($"api/user/deleteuser?userId={userId}&&token={userToken}");
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
        ////test alex
        //public async Task UpdateUserModel(UserModel updatedUser)
        //{
        //    await _httpClient.PutAsJsonAsync<UserModel>("api/user", updatedUser);
        //}

        public async Task UpdatePassword(PasswordDTOModel UserToUpdate)
        {
            await _httpClient.PutAsJsonAsync<PasswordDTOModel>("api/user", UserToUpdate);
        }

        public async Task ChangeDeletedStateToTrue(UserModel UserToChange)
        {
            await _httpClient.PutAsJsonAsync<UserModel>("api/user/cstt", UserToChange);
        }
         public async Task ChangeDeletedStateToFalse(UserModel UserToChange)
        {
            await _httpClient.PutAsJsonAsync<UserModel>("api/user/cstf", UserToChange);
        }

    }
}
