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
using Java.Lang.Annotation;
using System.Threading;

namespace MT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoConnectionPage : ContentPage
	{
        mysqldatabase mysqldatabase;
		public NoConnectionPage (App MainApplication)
		{
			InitializeComponent ();
            settingsserver.Text = connectionsettings.server;
            settingsport.Text = connectionsettings.port;
            settingsuserid.Text = connectionsettings.userid;
            settingspassword.Text = connectionsettings.password;
            settingsdatabase.Text = connectionsettings.database;
            mysqldatabase = new mysqldatabase();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //TryConnection();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            TryConnection();
        }
        public async void TryConnection()
        {
            UserDialogs.Instance.ShowLoading("Connecting to database...");
            await mysqldatabase.tryConnectionAsync(settingsserver.Text, settingsuserid.Text, settingspassword.Text, settingsdatabase.Text, uint.Parse(settingsport.Text));
            UserDialogs.Instance.HideLoading();
        }
    }
}