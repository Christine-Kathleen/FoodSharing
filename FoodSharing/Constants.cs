using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FoodSharing
{
    public static class Constants
    {
        public static string FoodUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Foods/{0}" : "http://localhost:44318/api/Foods/{0}";

        public static string LoginUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/login" : "http://localhost:44318/api/authenticate/login";
        public static string RegisterUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/register" : "http://localhost:44318/api/authenticate/register";
        public static string GetUserUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/GetUser" : "http://localhost:44318/api/authenticate/GetUser";
        public static string DeleteUserUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/DeleteUser" : "http://localhost:44318/api/authenticate/DeleteUser";  
        public static string UpdateUserProfileUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/UpdateUserProfile" : "http://localhost:44318/api/Foods/UpdateUserProfile";
        public static string UpdateUserPassUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/UpdateUserPassword" : "http://localhost:44318/api/authenticate/UpdateUserPassword";

        public static string ReviewUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Reviews/{0}" : "http://localhost:44318/api/Reviews/{0}";
        public static string MessageUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Messages/{0}" : "http://localhost:44318/api/Messages/{0}";

        public static string connectionString = "DefaultEndpointsProtocol=https;" +
                                                "AccountName=foodsharingimages;" +
                                                "AccountKey=ONGnTrShMj4G6r2baZ6QcD/zRSzSl9TgCx6lkXfQYzvK4DKUTbrwHNCw4v0F+2aKQMOpCsNEV4tFJ7N5zb6Ocw==;" +
                                                "EndpointSuffix=core.windows.net";

        public enum APIMessages
        {
            ErrorRegisterName,
            ErrorRegisterEmail,
            ErrorOnRegisterFailed,
            Success,
            ErrorOnDeletion,
            ErrorOnCreating,
            ErrorAlreadyExists,
            ErrorOnPasswordChange, 
            ErrorOnPasswordCheck,
            ErrorOnUpdate,
            ErrorOnNotFound

        }
        public enum Status
        {
            Error,
            Success
        }
    }
}