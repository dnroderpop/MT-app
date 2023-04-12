using MT.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MT.Models;
using static System.Net.Mime.MediaTypeNames;
using Acr.UserDialogs;
using System.Threading;

namespace MT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoConnectionPage : ContentPage
    {
        mysqldatabase mysqldatabase;
        public NoConnectionPage(App MainApplication)
        {
            InitializeComponent();
            mysqldatabase = new mysqldatabase();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //check for setting availability
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("server"))
            {
                Xamarin.Forms.Application.Current.Properties["server"] = "122.54.146.208";
                Xamarin.Forms.Application.Current.Properties["port"] = "3306";
                Xamarin.Forms.Application.Current.Properties["userid"] = "rodericks";
                Xamarin.Forms.Application.Current.Properties["password"] = "mtchoco";
                Xamarin.Forms.Application.Current.Properties["database"] = "mangtinapay";
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }

            settingsserver.Text = Xamarin.Forms.Application.Current.Properties["server"].ToString();
            settingsport.Text = Xamarin.Forms.Application.Current.Properties["port"].ToString();
            settingsuserid.Text = Xamarin.Forms.Application.Current.Properties["userid"].ToString();
            settingspassword.Text = Xamarin.Forms.Application.Current.Properties["password"].ToString();
            settingsdatabase.Text = Xamarin.Forms.Application.Current.Properties["database"].ToString();

            TryConnection();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Xamarin.Forms.Application.Current.Properties["server"] = settingsserver.Text;
            Xamarin.Forms.Application.Current.Properties["port"] = settingsport.Text;
            Xamarin.Forms.Application.Current.Properties["userid"] = settingsuserid.Text;
            Xamarin.Forms.Application.Current.Properties["password"] = settingspassword.Text;
            Xamarin.Forms.Application.Current.Properties["database"] = settingsdatabase.Text;
            Xamarin.Forms.Application.Current.SavePropertiesAsync();

            TryConnection();
        }
        public async void TryConnection()
        {
            //save settings


            UserDialogs.Instance.ShowLoading("Connecting to database...",maskType:MaskType.Black);
            await mysqldatabase.tryConnectionAsync(settingsserver.Text, settingsuserid.Text, settingspassword.Text, settingsdatabase.Text, uint.Parse(settingsport.Text)).ConfigureAwait(true);
            UserDialogs.Instance.HideLoading();
        }
    }
}