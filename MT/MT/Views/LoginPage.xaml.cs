using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MT.ViewModels;
using MT.Views;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Acr.UserDialogs;
using MT.Services;

namespace MT
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            UserDialogs.Instance.HideLoading();
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            UserDialogs.Instance.ShowLoading("Retrieving data from commissary");
            mysqldatabase mysqldatabase = new mysqldatabase();
            await mysqldatabase.loadbranchandproducts();
        }

    }
}
