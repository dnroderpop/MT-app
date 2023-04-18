using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using MT.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public partial class loginpageviewmodel : ObservableObject
    {
        public loginpageviewmodel()
        {
            mysqlget = new mysqlGET();
        }
        private mysqlGET mysqlget;

        [ObservableProperty]
        string username;

        [ObservableProperty]
        string password;

        [RelayCommand]
        void register()
        {
            App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        [RelayCommand]
        async void submit()
        {
            UserDialogs.Instance.ShowLoading("Logging In");
            string result = await mysqlget.querySingleStringFromDatabase("user_accounts", "id", "user", "password", Username, Password);
            if (result != null)
            {


                userloginProfileModel userloginProfile = mysqlget.mysqlloadLoggedUserInfo(int.Parse(result));


                if (userloginProfile.Branchid == 21)
                    App.Current.MainPage = new CommiOrderPage();
                else
                    App.Current.MainPage = new BranchOrderPage();


                UserDialogs.Instance.HideLoading();
            }
            else
                UserDialogs.Instance.Alert("Incorrect Username or Password");

        }


        
    }
}
