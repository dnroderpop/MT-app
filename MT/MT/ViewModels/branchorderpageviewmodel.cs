using Acr.UserDialogs;
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
        bool isSearching = false;

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

        [RelayCommand]
        void onLogout()
        {
            Preferences.Set("islogged", false);
            Application.Current.SavePropertiesAsync();
            App.Current.MainPage = new LoginPage();
        }

        internal void OnAppearing()
        {
            onPulltoRefresh();
        }

        [RelayCommand]
        void editButton(productOrderModel selected)
        {
            UserDialogs.Instance.Toast(selected.Id.ToString());
            if (selected == null) return;

            double selectedEditNumber = 0;

            UserDialogs.Instance.Prompt(new PromptConfig()
            {
                Title = selected.ProductName,
                Message = "Current Quantity is " + selected.Qty + " pcs",
                Placeholder = "Decimal number representing your order",
                InputType = InputType.DecimalNumber,
                OnAction = (result) =>
                {
                    if(result.Ok)
                    selectedEditNumber = double.Parse(result.Value);
                }
            });
        }

        [RelayCommand]
        internal async void onPulltoRefresh()
        {
            IsBusy = await Task.Run<bool>(() => {
                Products = new ObservableGroupedCollection<string, productOrderModel>();
                Products.Clear();
                var listprod = mysqlget.getproductorder(false, DateOrder, Branchid).ToList<productOrderModel>();

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
        void SearchProduct()
        {
            IsSearching = true;
        }

        [RelayCommand]
        void AddProduct()
        {
            IsSearching = false;
        }
    }
}
