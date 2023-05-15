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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public partial class branchorderpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;
        [ObservableProperty]
        bool isSearching;

        [ObservableProperty]
        DateTime dateOrder;
        [ObservableProperty]
        string productsearchtext;
        [ObservableProperty]
        string branchName;
        [ObservableProperty]
        int branchid;
        [ObservableProperty]
        double total;
        [ObservableProperty]
        productProfileModel selectedproduct;

        [ObservableProperty]
        ObservableGroupedCollection<string, productOrderModel> products = new ObservableGroupedCollection<string, productOrderModel>();
        [ObservableProperty]
        List<productProfileModel> showproductlist;

        mysqldatabase mysqldatabase = new mysqldatabase();
        mysqlGET mysqlget = new mysqlGET();
        userloginProfileModel userloginProfile;
        List<productProfileModel> productProfileModels;
        bool istemp = true;

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

        internal async Task OnAppearing()
        {
            UserDialogs.Instance.ShowLoading("Fetching Data");

            await onPulltoRefresh();
            await loadProducts();

            UserDialogs.Instance.HideLoading();
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
                OnAction = async (result) =>
                {
                    if (result.Ok)
                    {
                        selectedEditNumber = double.Parse(result.Value);
                        mysqlUPDATE mysqlUPDATE = new mysqlUPDATE();
                        mysqlUPDATE.updateqtyProductOrder(istemp, selected.Id, selectedEditNumber);
                        await onPulltoRefresh();
                    }
                }
            });
        }

        [RelayCommand]
        async void deleteButton(productOrderModel selected)
        {
            if (selected == null) return;

            var check = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
            {
                Message = "Are you sure you want to delete " + selected.ProductName + " ?",
                Title = "Delete Product",
                OkText = "Delete",
                CancelText = "Cancel",
            });

            //Check if cancel button is press
            if (!check) return;

            mysqDELETE mysqldel = new mysqDELETE();
            mysqldel.deleteProductOrder(istemp, selected.Id);
            await onPulltoRefresh();

        }


        [RelayCommand]
        internal async Task onPulltoRefresh()
        {
            try
            {
                Products = new ObservableGroupedCollection<string, productOrderModel>();
                Products.Clear();
                await Task.Delay(1000); //delay for 1 second to show responsiveness
                var listprod = (mysqlget.getproductorder(istemp, DateOrder, Branchid)).ToList<productOrderModel>();

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
            }
            finally
            {
                IsBusy = false;
            }
            

        }


        [RelayCommand]
        void SearchProduct()
        {

            if (string.IsNullOrEmpty(Productsearchtext)) return;

            IsSearching = true;
            Showproductlist = (List<productProfileModel>)productProfileModels.Where(s => s.Name.Contains(Productsearchtext, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        [RelayCommand]
        async Task AddProduct()
        {
            if (Selectedproduct == null) return;

            IsBusy = true;

            //check if added already
            if (await mysqlget.checkIfDuplicateProduct(istemp, DateOrder, userloginProfile.Branchid, Selectedproduct.Id))
            {
                UserDialogs.Instance.Toast("Duplicate Product Detected");
                IsBusy = false;
                return;
            }

            var check = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
            {
                Message = "Are you sure to add this " + Selectedproduct.Name.ToString() + " at " + Selectedproduct.Srpc,
                Title = "Adding Product",
                OkText = "Add",
                CancelText = "Cancel"
            });

            IsBusy = false;
            //Check if cancel button is press
            if (!check) return;

            IsBusy = true;
            mysqlINSERT mysqlINSERT = new mysqlINSERT();
            await mysqlINSERT.addProductOrder(istemp, DateOrder, userloginProfile.Branchid, 1, Selectedproduct.Id);

            Selectedproduct = null;
            IsSearching = false;
            IsBusy = false;
            await onPulltoRefresh();
        }

        async Task loadProducts()
        {
            await mysqldatabase.loadbranchandproducts();
            loadedProfileModel loadedProfile = mysqldatabase.getBranchandproducts();
            productProfileModels = loadedProfile.productProfileModels;
        }
    }
}
