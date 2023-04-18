using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        DateTime dateOrder;

        [ObservableProperty]
        public ObservableCollection<productOrderModel> productOrderModels;


        mysqlGET mysqlget = new mysqlGET();
        mysqldatabase mysqldatabase = new mysqldatabase();
        userloginProfileModel userloginProfile;

        public void branchoderpageviewmodel()
        {
            userloginProfile = (userloginProfileModel)Application.Current.Properties["loggedin"];
            int branchid = userloginProfile.Branchid;
            productOrderModels.Clear();
            productOrderModels = mysqlget.getproductorder(false, DateOrder, branchid );
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
