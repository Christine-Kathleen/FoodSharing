﻿using FoodSharing.ViewModels;
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
    public partial class FromStorePage : ContentPage
    {
        public FromStorePage()
        {
            InitializeComponent();
            BindingContext = new FromStoreViewModel(Models.TypeOfFood.FromStore);
        }
    }
}