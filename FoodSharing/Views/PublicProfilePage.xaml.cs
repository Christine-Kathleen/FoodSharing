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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicProfilePage : ContentPage
    {
        public PublicProfilePage()
        {
            var vm = new PublicProfileViewModel();
            this.BindingContext = vm;
            InitializeComponent();
        }
    }
}