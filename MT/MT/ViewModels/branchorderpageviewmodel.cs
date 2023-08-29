using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Nio.Channels;
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
        public bool IsBusy { get; set; }

        [ObservableProperty]
        bool isSearching;
        [ObservableProperty]
        bool showApproved;

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
        mysqlINSERT mysqlINSERT = new mysqlINSERT();
        mysqDELETE mysqlDelete = new mysqDELETE();
        userloginProfileModel userloginProfile;
        List<productProfileModel> productProfileModels;
        bool istemp = true;

        public branchorderpageviewmodel()
        {
            userloginProfile = (userloginProfileModel)Application.Current.Properties["loggedin"];
            Branchid = userloginProfile.Branchid;
            BranchName = userloginProfile.Branchname;
            ShowApproved = false;
        }

        [RelayCommand]
        void onLogout()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
            {
                Title = "Menu",
                Destructive = new ActionSheetOption("Back"),
                UseBottomSheet = false,
                Options = {
                new ActionSheetOption("Saved current orders", () => {
                    SaveCurrentOrder();
                },null),
                new ActionSheetOption("Load saved orders", () => {
                    loadSavedOrders();
                },null),
                new ActionSheetOption("Delete All Orders", () =>
                {
                    deleteallorders();
                },null)
                ,
                new ActionSheetOption("Logout", () => {
                    Preferences.Set("islogged", false);
                    Application.Current.SavePropertiesAsync();
                    App.Current.MainPage = new LoginPage();},null)
                }
            });
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
                        if (result.Value == "" || result.Value == null)
                            selectedEditNumber = selected.Qty;
                        else
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

        void SaveCurrentOrder()
        {
            UserDialogs.Instance.Confirm(new ConfirmConfig()
            {
                Title = "ARE YOU SURE?",
                Message = "this will replace saved orders if you have any saved orders, are you sure?",
                OkText = "save Orders",
                CancelText = "Cancel",
                OnAction = async (result) =>
                {
                    if (result == true)
                    {
                        if (Products.Count >= 0)
                        {
                            UserDialogs.Instance.ShowLoading("Saving Orders");

                            await Task.Delay(500); //delay for 1 second to show responsiveness

                            mysqlDelete.deleteSavedOrders(Branchid);
                            mysqlINSERT.onSaveCurrentOrders(Branchid, DateOrder);

                            UserDialogs.Instance.Toast("Saved");
                            UserDialogs.Instance.HideLoading();
                            await onPulltoRefresh();
                        }
                        else
                        {
                            UserDialogs.Instance.Confirm(new ConfirmConfig()
                            {
                                Title = "No Items",
                                Message = "Would you like to clear saved to default?",
                                OkText = "Default Order",
                                CancelText = "Cancel",
                                OnAction = async (result) =>
                                {
                                    if (result == true)
                                    {
                                        if (Products.Count >= 0)
                                        {
                                            UserDialogs.Instance.ShowLoading("Saving Orders");

                                            await Task.Delay(500); //delay for 1 second to show responsiveness

                                            mysqlDelete.deleteSavedOrders(Branchid);

                                            UserDialogs.Instance.Toast("Saved");
                                            UserDialogs.Instance.HideLoading();
                                            await onPulltoRefresh();
                                        }
                                        else
                                        {
                                            UserDialogs.Instance.Toast("No item To save");
                                        }
                                    }
                                }
                            });

                        }
                    }
                }

            });
        }

        async void loadSavedOrders()
        {
            UserDialogs.Instance.ShowLoading("Loading Saved Orders");
            List<savedOrderModel> savedorders = mysqlget.getSavedOrders(Branchid);
            await Task.Delay(500); //delay for 1 second to show responsiveness
            foreach (savedOrderModel order in savedorders)
            {
                await mysqlINSERT.addProductOrder(istemp, DateOrder, Branchid, order.Quantity, order.ProductID);
            }
            UserDialogs.Instance.HideLoading();
            await onPulltoRefresh();
        }

        void deleteallorders()
        {
            UserDialogs.Instance.Confirm(new ConfirmConfig()
            {
                Title = "ARE YOU SURE?",
                Message = "THIS WILL DELETE ALL NOT APPROVED ORDERS AND IS NOT REVERSABLE! ARE YOU SURE?",
                OkText = "Delete permanently",
                CancelText = "Cancel",
                OnAction = async (result) =>
                {
                    if (result == true)
                    {
                        UserDialogs.Instance.ShowLoading("Deleting Orders");

                        await Task.Delay(500); //delay for 1 second to show responsiveness
                        mysqlDelete.deleteAllProductOrder(DateOrder, Branchid);
                        UserDialogs.Instance.Toast("deleted");
                        UserDialogs.Instance.HideLoading();
                        await onPulltoRefresh();
                    }
                }
            });
        }

        [RelayCommand]
        internal async Task onPulltoRefresh()
        {
            UserDialogs.Instance.ShowLoading("Fetching Data");
            try
            {
                Products = new ObservableGroupedCollection<string, productOrderModel>();
                Products.Clear();
                await Task.Delay(500); //delay for 1 second to show responsiveness
                var listprod = (mysqlget.getproductorder(istemp, DateOrder, Branchid, ShowApproved)).ToList<productOrderModel>();

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


            //Check if cancel button is press
            if (!check) { IsBusy = false; return; }

            double selectedEditNumber = 0;

            UserDialogs.Instance.Prompt(new PromptConfig()
            {
                Title = Selectedproduct.Name,
                Message = "Current Quantity is 0 pcs",
                Placeholder = "Decimal number representing your order",
                InputType = InputType.DecimalNumber,
                OnAction = async (result) =>
                {
                    if (result.Ok)
                    {
                        if (result.Value == "" || result.Value == null)
                            selectedEditNumber = 0;
                        else
                            selectedEditNumber = double.Parse(result.Value);

                        mysqlUPDATE mysqlUPDATE = new mysqlUPDATE();
                        mysqlUPDATE.updateqtyProductOrder(istemp, Selectedproduct.Id, selectedEditNumber);

                        mysqlINSERT mysqlINSERT = new mysqlINSERT();
                        await mysqlINSERT.addProductOrder(istemp, DateOrder, Branchid, selectedEditNumber, Selectedproduct.Id);

                        Selectedproduct = null;
                        IsSearching = false;
                        IsBusy = false;
                        await onPulltoRefresh();
                    }
                    else { IsBusy = false; return; }
                }
            });





        }

        async Task loadProducts()
        {
            await mysqldatabase.loadbranchandproducts();
            loadedProfileModel loadedProfile = mysqldatabase.getBranchandproducts();
            productProfileModels = loadedProfile.productProfileModels;
        }
    }
}
