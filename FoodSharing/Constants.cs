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
        public static string DeleteUserUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/DeleteUser" : "http://localhost:44318/api/authenticate/DeleteUser";
        public static string DeleteFoodUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Foods/DeleteFood" : "http://localhost:44318/api/Foods/DeleteFood";
        public static string UpdateUserProfileUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/UpdateUserProfile" : "http://localhost:44318/api/Foods/UpdateUserProfile";
        public static string UpdateUserPassUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/authenticate/UpdateUserPassword" : "http://localhost:44318/api/authenticate/UpdateUserPassword";
        //public static string GetFoodUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Foods/GetFood" : "http://localhost:44318/api/Foods/GetFood";
        public static string ReviewUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Reviews/{0}" : "http://localhost:44318/api/Reviews/{0}";
        public static string MessageUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://foodsharingmobile.azurewebsites.net/api/Messages/{0}" : "http://localhost:44318/api/Messages/{0}";

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
            ErrorOnUpdate

        }

        public enum Status
        {
            Error,
            Success
        }
    }
}