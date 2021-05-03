using FoodSharing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

namespace FoodSharing.Services
{
    public class RestService : IRestService
    {
        HttpClient client;
        JsonSerializerOptions serializerOptions;
        public string BearerToken => Preferences.Get("BearerToken", string.Empty);
        public RestService()
        {
                client = new HttpClient();
                serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
        }


        public async Task<ApplicationUser> GetUser(string username, string password)
        {
            ApplicationUser user = new ApplicationUser();
           
            dynamic jsonObject = new JObject();
            jsonObject.Username = username;
            jsonObject.Password = password;
            try
            {
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                var responseMessage = await client.PostAsync(Constants.GetUserUrl, content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string content2 = await responseMessage.Content.ReadAsStringAsync();
                    user = System.Text.Json.JsonSerializer.Deserialize<ApplicationUser>(content2, serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return user;
        }
        public async Task SaveUserAsync(ApplicationUser user, bool isNewUser)
        {
            Uri uri = new Uri(string.Format(Constants.GetUserUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<ApplicationUser>(user, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");

                HttpResponseMessage response = null;
                if (isNewUser)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\food successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
        public async Task DeleteUserAsync(string id)
        {
            Uri uri = new Uri(string.Format(Constants.DeleteUserUrl));
            DeleteUserModel userModel = new DeleteUserModel();
            userModel.UserId = id;

            try
            {
                string json = JsonSerializer.Serialize<DeleteUserModel>(userModel, serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.PostAsync(uri,content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\user successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task<bool> AuthWithCredentialsAsync(string username, string password)
        {
           
            dynamic jsonObject = new JObject();
            jsonObject.Username = username;
            jsonObject.Password = password;

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(Constants.LoginUrl, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                var stringResponse = await responseMessage.Content.ReadAsStringAsync();
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(stringResponse);

                Preferences.Set("BearerToken", authResponse.Token);
                //Preferences.Set("RefreshToken", authResponse.RefreshToken);

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Food> Foods { get; set; }

        public async Task<List<Food>> RefreshDataAsync()
        {
            Foods = new List<Food>();

            Uri uri = new Uri(string.Format(Constants.FoodUrl, string.Empty));
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Foods = System.Text.Json.JsonSerializer.Deserialize<List<Food>>(content, serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Foods;
        }

        public async Task SaveFoodAsync(Food food, bool isNewItem)
        {
            Uri uri = new Uri(string.Format(Constants.FoodUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<Food>(food, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");


                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\food successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task DeleteFoodAsync(int id)
        {
            Uri uri = new Uri(string.Format(Constants.FoodUrl, id));

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\food successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}