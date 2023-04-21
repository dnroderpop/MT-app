using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Lang;
using MT.Models;
using MT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        [ObservableProperty]
        double total;

        [ObservableProperty]
        ObservableGroupedCollection<string, productOrderModel> products = new ObservableGroupedCollection<string, productOrderModel>();

        mysqlGET mysqlget = new mysqlGET();
        userloginProfileModel userloginProfile;

        public branchorderpageviewmodel()
        {
            userloginProfile = (userloginProfileModel)Application.Current.Properties["loggedin"];
            Branchid = userloginProfile.Branchid;
            BranchName = userloginProfile.Branchname;

        }
        internal void OnAppearing()
        {
            onPulltoRefresh();
        }

        [RelayCommand]
        internal async void onPulltoRefresh()
        {
            IsBusy = await Task.Run<bool>(() => {
                Products.Clear();
                var listprod = mysqlget.getproductorder(true, DateOrder, Branchid).ToList<productOrderModel>();

                Total = 0;
                string category = "";
                foreach (productOrderModel model in listprod)
                {
                    if (category == "" || category != model.ProductCategory)
                    {
                        category = model.ProductCategory;
                        Products.AddGroup(category);
                        Products.AddItem(category, model);
                    }
                    else if (category == model.ProductCategory)
                    {
                        Products.AddItem(category, model);
                    }
                    Total += model.Tamount;
                }

                return false;
            });
            

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
