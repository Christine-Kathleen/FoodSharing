using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class MasterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public ICommand HomeCommand { protected set; get; }
        public ICommand MyProfileCommand { protected set; get; }
        public ICommand AccountSettingsCommand { protected set; get; }
        public ICommand HelpGrowCommand { protected set; get; }
        public ICommand LogoutCommand { protected set; get; }
        

        public MasterViewModel()
        {
            HomeCommand = new Command(OnHome);
            MyProfileCommand = new Command(OnMyProfile);
            AccountSettingsCommand = new Command(OnAccountSettings);
            HelpGrowCommand = new Command(OnHelpGrow);
            LogoutCommand = new Command(OnLogout);
        }

        public void OnHome()
        {

        }
        public void OnMyProfile()
        {

        }

        public void OnAccountSettings()
        {

        }
        public void OnHelpGrow()
        {

        }
        public void OnLogout()
        {

        }

    }
}
