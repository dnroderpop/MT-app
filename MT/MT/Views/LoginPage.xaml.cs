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

    }
}
