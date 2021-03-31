using FoodSharing.Models;
using FoodSharing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Pages
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage ()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            this.BindingContext = vm;
            this.Master = new MasterPage();
            this.Detail = new NavigationPage(new TabbedPageTest());
           
        }
    }
}