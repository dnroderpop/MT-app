using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public partial class branchorderpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy = false;

        public void branchoderpageviewmodel()
        {

        }

        [RelayCommand]
        void onPulltoRefresh()
        {
            IsBusy = true;

            Task.Delay(2000);

            IsBusy = false;
        }

        [RelayCommand]
        void onLogout()
        {
            Preferences.Set("islogged", false);
            Application.Current.SavePropertiesAsync();
            App.Current.MainPage = new LoginPage(); 
        }
        
    }
}
