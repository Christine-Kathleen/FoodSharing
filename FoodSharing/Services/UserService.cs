using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FoodSharing.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using WebAPI.Authentication;

namespace FoodSharing.Services
{
    public class UserService : IUserService
    {
        private const string URL = "http://192.168.1.4:4444/api/people";
        public IRESTService RESTService => DependencyService.Get<IRESTService>();
        public UserService()
        {
        }

        public Task<IEnumerable<ApplicationUser>> GetPeopleAsync()
        {
            return RESTService.ExecuteWithRetryAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {RESTService.BearerToken}");

                    var responseMessage = await client.GetAsync(URL);

                    responseMessage.EnsureSuccessStatusCode();

                    var jsonResponse = await responseMessage.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<IEnumerable<PersonInfo>>(jsonResponse);

                    return response;
                }
            });
        }


    }
}

