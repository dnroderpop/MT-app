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

namespace MT.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoConnectionPage : ContentPage
	{
        mysqldatabase mysqldatabase;
        connectionsettings connectionsettings;
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

            TryConnection();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            TryConnection();
        }
        public void TryConnection()
        {
            //save the settings strings
            //connectionsettings.database = settingsdatabase.Text;
            //connectionsettings.port =settingsport.Text ;
            //connectionsettings.userid = settingsuserid.Text;
            //connectionsettings.password = settingspassword.Text;
            //connectionsettings.database = settingsdatabase.Text;

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                var result = mysqldatabase.tryConnectionAsync(settingsserver.Text, settingsuserid.Text, settingspassword.Text, settingsdatabase.Text, uint.Parse(settingsport.Text)).Result;
                if (result)
                    App.Current.MainPage = new NavigationPage(new LoginPage());
            }else
                DisplayAlert("No internet", "No connection has been establish", "Okay");

        }
    }
}