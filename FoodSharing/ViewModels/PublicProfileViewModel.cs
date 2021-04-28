using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class PublicProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        //public ICommand PublicProfileCommand { protected set; get; }

        public PublicProfileViewModel()
        {
            //PublicProfileCommand = new Command(OnPublicProfile);
        }

    }
}
