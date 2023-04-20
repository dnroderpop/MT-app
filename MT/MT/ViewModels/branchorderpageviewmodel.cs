using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        string branchName;
        [ObservableProperty]
        int branchid;

        public ObservableCollection<productOrderModel> productOrderModels { get; set; }


        mysqlGET mysqlget = new mysqlGET();
        mysqldatabase mysqldatabase = new mysqldatabase();
        userloginProfileModel userloginProfile;

        public branchorderpageviewmodel()
        {
            userloginProfile = (userloginProfileModel)Application.Current.Properties["loggedin"];
            Branchid = userloginProfile.Branchid;
            BranchName = userloginProfile.Branchname;
            productOrderModels = new ObservableCollection<productOrderModel>();
        }

        [RelayCommand]
        internal async void onPulltoRefresh()
        {
           
            if  (IsBusy) return;
            await Task.Run(() =>
            {
                IsBusy = true;
                if (productOrderModels.Count() != 0)
                    productOrderModels.Clear();
                var listprod = mysqlget.getproductorder(false, DateOrder, Branchid).ToList<productOrderModel>();
                //productOrderModels = new ObservableCollection<productOrderModel>(listprod);
                foreach (productOrderModel model in listprod)
                {
                    productOrderModels.Add(model);
                }
            });


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
