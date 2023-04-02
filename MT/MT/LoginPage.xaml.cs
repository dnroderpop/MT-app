﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MT.ViewModels;
using MT.Views;

namespace MT
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            loginpageviewmodel loginpage = new loginpageviewmodel();

            login_username.BindingContext = loginpage;
            login_username.SetBinding(ContentProperty, "username");
            login_password.SetBinding(ContentProperty, "password");
        }

        private void Register_tapped(object sender, EventArgs e)
        {
            login_newNavigation(new RegisterPage(),false);
        }

        private  void login_click(object sender, EventArgs e)
        {
           login_newNavigation(new BranchOrderPage(),true);  

        }

        private async void login_newNavigation(Page destination,bool playanimation)
        {
            await Navigation.PushAsync(destination, playanimation);
        }
    }
}
