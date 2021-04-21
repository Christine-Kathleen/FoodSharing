using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FoodSharing
{
    public static class Constants
    {
        public static string RestUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Foods/{0}" : "https://localhost:44318/api/Foods/{0}";
    }
}