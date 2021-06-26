using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpGrowPage : ContentPage
    {
        public HelpGrowPage()
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(51, 153, 137);
            InitializeComponent();
        }
    }
}