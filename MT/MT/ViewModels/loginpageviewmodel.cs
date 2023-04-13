using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public partial class loginpageviewmodel : ObservableObject
    {

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
        void submit()
        {
            if (this.Username == "1")
                App.Current.MainPage = new BranchOrderPage();
            else
                App.Current.MainPage = new CommiOrderPage();
        }

    }
}
