﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MT.ViewModels;
using MT.Views;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Acr.UserDialogs;

namespace MT
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            UserDialogs.Instance.HideLoading();
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        private void Register_tapped(object sender, EventArgs e)
        {
            login_newNavigation(new RegisterPage(),false);
        }

        private  void login_click(object sender, EventArgs e)
        {
           if(login_username.Text != "")
                login_newNavigation(new BranchOrderPage(),true); 
           else
                login_newNavigation(new CommiOrderPage(), true);

        }

        private async void login_newNavigation(Page destination,bool playanimation)
        {
             Navigation.PushAsync(destination, false);
        }
    }
}
