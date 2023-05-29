using Acr.UserDialogs;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public partial class commiorderpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;

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

        mysqldatabase mysqldatabase;
        mysqlGET mysqlget = new mysqlGET();
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

        //[RelayCommand]
        //void editButton(orderProfileModel selected)
        //{
        //}
        //
        //[RelayCommand]
        //void approveButton(orderProfileModel selected)
        //{
        //}


        [RelayCommand]
        internal async Task onPulltoRefresh()
        {
            try
            {
                GroupOrder = new ObservableGroupedCollection<string, orderProfileModel>();
                GroupOrder.Clear();
                await Task.Delay(1000); //delay for 1 second to show responsiveness
                var listprod = mysqlget.getOrders(DateOrder).ToList<orderProfileModel>();

                Total = 0;
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
            }


        }


    }
}
