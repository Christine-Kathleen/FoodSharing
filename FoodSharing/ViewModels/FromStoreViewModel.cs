using System.Collections.Generic;
using Azure.Storage.Blobs;
using FoodSharing.Models;
using FoodSharing.Pages;
using FoodSharing.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodSharing.ViewModels
{
    public class FromStoreViewModel : BaseFoodViewModel
    {
        public FromStoreViewModel(TypeOfFood type) : base(type)
        {

        }
    }
}


