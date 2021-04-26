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
    public partial class TabbedPageTest : TabbedPage
    {
        public TabbedPageTest()
        {
            InitializeComponent();
            var vm = new TabbedViewModel();
            this.BindingContext = vm;
            //var clr = Color.FromHex("#FAFAFB");
            //this.BarBackgroundColor = clr;
        }
    }
}