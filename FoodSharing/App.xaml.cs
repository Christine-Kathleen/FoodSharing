using FoodSharing.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FoodSharing
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new string[] { "RadioButton_Experimental" });
            InitializeComponent();
            //LoginPage Page = new LoginPage();
            //Application.Current.MainPage = Page;
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
       
    }
}
