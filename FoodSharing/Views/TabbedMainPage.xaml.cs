using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodSharing.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMainPage : TabbedPage
    {
        public TabbedMainPage()
        {
            InitializeComponent();
            BindingContext = this;
            var clr = Color.FromHex("#339989");
            var clrtext = Color.FromHex("#2B2C28");
            var clrselect = Color.FromHex("#FFFAFB");
            this.BarBackgroundColor = clr;
            this.BarTextColor = clrtext;
            this.SelectedTabColor = clrselect;
        }
    }
}