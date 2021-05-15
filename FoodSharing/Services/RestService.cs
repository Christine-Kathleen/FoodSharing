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
        public async Task<Response> RegisterUserAsync(RegisterModel model)
        {
            Uri uri = new Uri(string.Format(Constants.RegisterUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<RegisterModel>(model, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\user successfully created.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
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
                    Debug.WriteLine(@"\user successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
        public async Task<Response> UpdatePasswordAsync(UpdatePasswordModel model)
        {
            Uri uri = new Uri(string.Format(Constants.UpdateUserPassUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<UpdatePasswordModel>(model, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");

                HttpResponseMessage response = null;
                response = await client.PatchAsync(uri, content);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\user password successfully saved.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }
        public async Task<Response> UpdateUserAsync(UpdateUserModel model)
        {
            Uri uri = new Uri(string.Format(Constants.UpdateUserProfileUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<UpdateUserModel>(model, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");

                HttpResponseMessage response = null;
                response = await client.PatchAsync(uri, content);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\user password successfully saved.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<Response> DeleteUserAsync(string id)
        {
            Uri uri = new Uri(string.Format(Constants.DeleteUserUrl));
            DeleteUserModel userModel = new DeleteUserModel();
            userModel.UserId = id;

            try
            {
                string json = JsonSerializer.Serialize<DeleteUserModel>(userModel, serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\user successfully deleted.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
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

        public async Task<List<Food>> RefreshFoodDataAsync()
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

        public async Task<Response> SaveFoodAsync(Food food, bool isNewItem)
        {
            Uri uri = new Uri(string.Format(Constants.FoodUrl, isNewItem?string.Empty:food.FoodId.ToString()));

            try
            {
                food.User = null;
                food.ImageSource = null;
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
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\food successfully saved.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }

        public async Task<Response> DeleteFoodAsync(int id)
        {
            Uri uri = new Uri(string.Format(Constants.FoodUrl, id));
            //DeleteFoodModel foodModel = new DeleteFoodModel();
            //foodModel.FoodId = id;

            try
            {
                //string json = JsonSerializer.Serialize<DeleteFoodModel>(foodModel, serializerOptions);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.DeleteAsync(uri);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\food successfully deleted.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }
       // public List<Review> Reviews { get; set; }

        public async Task<List<Review>> RefreshReviewDataAsync(string ReviewedUserId)
        {
            List<Review>  Reviews = new List<Review>();

            Uri uri = new Uri(string.Format(Constants.ReviewUrl, ReviewedUserId));
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode && response.Content!=null)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                        Reviews = System.Text.Json.JsonSerializer.Deserialize<List<Review>>(content, serializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Reviews;
        }
        public async Task<Response> SaveReviewAsync(Review review, bool isNewItem)
        {
            Uri uri = new Uri(string.Format(Constants.ReviewUrl, isNewItem?string.Empty:review.ReviewId.ToString()));

            try
            {
                string json = JsonSerializer.Serialize<Review>(review, serializerOptions);
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
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\review successfully saved.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }
        //public List<Message> Messages { get; set; }

        public async Task<List<Message>> RefreshMessageDataAsync(string UserId)
        {
            List<Message>  Messages = new List<Message>();

            Uri uri = new Uri(string.Format(Constants.MessageUrl, UserId));
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        Messages = System.Text.Json.JsonSerializer.Deserialize<List<Message>>(content, serializerOptions);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Messages;
        }

        public async Task<Response> SaveMessageAsync(Message message, bool isNewItem)
        {
            Uri uri = new Uri(string.Format(Constants.MessageUrl, string.Empty));

            try
            {
                string json = JsonSerializer.Serialize<Message>(message, serializerOptions);
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
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\message successfully saved.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }
        public async Task<Response> DeleteMessageAsync(int id)
        {
            Uri uri = new Uri(string.Format(Constants.MessageUrl, id));
            DeleteFoodModel foodModel = new DeleteFoodModel();
            foodModel.FoodId = id;

            try
            {
                string json = JsonSerializer.Serialize<DeleteFoodModel>(foodModel, serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.BearerToken}");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                string jsonresponse = await response.Content.ReadAsStringAsync();
                Response response2 = System.Text.Json.JsonSerializer.Deserialize<Response>(jsonresponse, serializerOptions);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\food successfully deleted.");
                }
                return response2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return null;
            }
        }
    }    
}
