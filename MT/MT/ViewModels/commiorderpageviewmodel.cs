﻿using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Nio.Channels;
using MT.Models;
using MT.Services;
using MT.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Content.ClipData;
using Application = Xamarin.Forms.Application;

namespace MT.ViewModels
{
    internal partial class commiorderpageviewmodel : ObservableObject
    {
        //initialization variables
        public bool IsBusy { get; set; }

        [ObservableProperty]
        DateTime dateOrder;
        [ObservableProperty]
        string branchName;
        [ObservableProperty]
        int branchid;
        [ObservableProperty]
        double total;

        [ObservableProperty]
        ObservableGroupedCollection<string, orderProfileModel> groupOrder = new ObservableGroupedCollection<string, orderProfileModel>();

        [ObservableProperty]
        orderProfileModel selecteditem;

        mysqldatabase mysqldatabase;
        mysqlGET mysqlget = new mysqlGET();
        mysqlUPDATE mysqlupdate = new mysqlUPDATE();
        userloginProfileModel userloginProfile;


        public commiorderpageviewmodel()
        {
            userloginProfile = (userloginProfileModel)Application.Current.Properties["loggedin"];
            Branchid = userloginProfile.Branchid;
            mysqldatabase = new mysqldatabase();
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

            UserDialogs.Instance.HideLoading();
        }

        [RelayCommand]
        void editButton(orderProfileModel selected)
        {
            Application.Current.Properties["selectedOrder"] = selected;
            Application.Current.MainPage.Navigation.PushModalAsync(new CommiEditOrderPage());
        }
        
        [RelayCommand]
        void approveButton(orderProfileModel selected)
        {
            UserDialogs.Instance.Confirm(new ConfirmConfig()
            {
                Title = "Confirming",
                OkText = "Approve",
                CancelText = "Not yet",
                Message = "Are you sure you want to Approve this " + selected.Items + "items order of " + selected.Branchname + "?",
                OnAction = (result) => {
                    ApprovingOrder(result,selected);
                }
            });
        }

        async void ApprovingOrder(bool result, orderProfileModel selected)
        {
            UserDialogs.Instance.ShowLoading("Approving the order");

            await Task.Delay(500);//to show responsiveness

            if (result == true)
            {
                var listprod = (mysqlget.getproductorder(true, selected.Date, selected.Branchid)).ToList<productOrderModel>();
                foreach (productOrderModel model in listprod)
                {
                    if (model.Ablebool != selected.IsAble) { }
                    else if (model.Qty <= 0)
                    {
                        mysqlupdate.updateNOorderapproval(model.Id);
                    }
                    else
                    {
                        mysqlupdate.updateorderapproval(model.Id);
                    }
                }

                await onPulltoRefresh();

            }

            UserDialogs.Instance.HideLoading();
        }

        partial void OnSelecteditemChanged(orderProfileModel value)
        {
            if (value == null) return;

            var orderlistinfo = new ActionSheetConfig();

            orderlistinfo.SetTitle(value.Branchname + " " + value.Date.ToString("MMM dd, yyyy"));
            orderlistinfo.SetDestructive("okay");

            //fetch data
            var listprod = (mysqlget.getproductorder(true, DateOrder, value.Branchid)).ToList<productOrderModel>();

            Total = 0;
            foreach (productOrderModel model in listprod)
            {
                if (value.IsAble == model.Ablebool)
                    orderlistinfo.Add(model.ProductName + " (" + model.Qty + "pcs), " + model.Tamount + " php");
            }

            UserDialogs.Instance.ActionSheet(orderlistinfo);

            Selecteditem = null;
        }

        [RelayCommand]
        async Task onPulltoRefresh()
        {
            UserDialogs.Instance.ShowLoading("Fetching Data");

            try
            {
                GroupOrder = new ObservableGroupedCollection<string, orderProfileModel>();
                GroupOrder.Clear();
                await Task.Delay(500); //delay for 1 second to show responsiveness
                var listprod = mysqlget.getOrders(DateOrder).ToList<orderProfileModel>();

                string status = "";
                foreach (orderProfileModel model in listprod)
                {
                    if (status == "" || status != model.Status)
                    {
                        status = model.Status;
                        GroupOrder.AddGroup(status);
                        GroupOrder.AddItem(status, model);
                    }
                    else if (status == model.Status)
                    {
                        GroupOrder.AddItem(status, model);
                    }
                }
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }


        }
    }
}
