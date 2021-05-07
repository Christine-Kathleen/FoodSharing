using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class HomeMadeViewModel : BaseFoodViewModel
    {
        public HomeMadeViewModel(TypeOfFood type):base(type)
        {
          
        }
    }
}

