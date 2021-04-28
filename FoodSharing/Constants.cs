using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FoodSharing
{
    public static class Constants
    {
//#if DEBUG
//        public static string FoodUrlDebug = "https://10.0.2.2/api/Foods/{0}";
//        public static string LoginUrlDebug = "http://10.0.2.2/api/authenticate/login";
//        public static string RegisterUrlDebug = "http://10.0.2.2/api/authenticate/register";
//        public static string GetUserUrlDebug = "http://10.0.2.2/api/authenticate/GetUser";

//#endif
        public static string FoodUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Foods/{0}" : "http://localhost:44318/api/Foods/{0}";
        public static string LoginUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/login" : "http://localhost:44318/api/authenticate/login";
        public static string RegisterUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/register" : "http://localhost:44318/api/authenticate/register";
        public static string GetUserUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/GetUser" : "http://localhost:44318/api/authenticate/GetUser";

    }
}