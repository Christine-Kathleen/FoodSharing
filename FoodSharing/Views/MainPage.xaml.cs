using FoodSharing.Models;
using FoodSharing.ViewModels;
using FoodSharing.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Pages
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage ()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            this.BindingContext = vm;
            this.Flyout = new MasterPage();
            this.Detail = new NavigationPage(new TabbedMainPage());
            ((NavigationPage)this.Detail).BarBackgroundColor = Color.FromHex("#339989");
            ((NavigationPage)this.Detail).BackgroundColor = Color.FromHex("#FFFAFB");
        }
    }
}