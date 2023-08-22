using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Linq;
using MT.Views;

namespace MT.ViewModels
{
    public partial class commieditorderviewmodel : ObservableObject
    {

        public bool IsBusy { get; set; }

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
        bool isAble;
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
        List<productProfileModel> productProfileModels;
        bool istemp = true;

        public commieditorderviewmodel()
        {
            orderProfileModel selectedOrder = (orderProfileModel)Application.Current.Properties["selectedOrder"];
            dateOrder = selectedOrder.Date;
            branchName = selectedOrder.Branchname;
            branchid = selectedOrder.Branchid;
            isAble = selectedOrder.IsAble;
        }

        [RelayCommand]
        void onBack()
        {
            App.Current.MainPage = new CommiOrderPage();
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
        async Task onPulltoRefresh()
        {
            UserDialogs.Instance.ShowLoading("Fetching Data");

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
                    if (model.Ablebool != IsAble) { }
                    else if (category == "" || category != model.ProductCategory)
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
                UserDialogs.Instance.HideLoading();
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
            if (await mysqlget.checkIfDuplicateProduct(istemp, DateOrder, Branchid, Selectedproduct.Id))
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


            //Check if cancel button is press
            if (!check) { IsBusy = false; return; }


            mysqlINSERT mysqlINSERT = new mysqlINSERT();
            await mysqlINSERT.addProductOrder(istemp, DateOrder, Branchid, 1, Selectedproduct.Id);

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
