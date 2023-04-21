using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using MT.Views;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public partial class noconnectionpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        string server;
        [ObservableProperty]
        string port;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        string database;
        [ObservableProperty]
        private bool isShow = false;

        mysqldatabase Mysqldatabase;

        public noconnectionpageviewmodel()
        {
            Mysqldatabase = new mysqldatabase();

            Server = Preferences.Get("server", "122.54.146.208");
            Port = Preferences.Get("port", "3306");
            Username = Preferences.Get("userid", "rodericks");
            Password = Preferences.Get("password", "mtchoco");
            Database = Preferences.Get("database", "mangtinapay");

            _ = tryConnection();
        }

        [RelayCommand]
        async Task tryConnection()
        {
            Preferences.Set("server", Server);
            Preferences.Set("port", Port);
            Preferences.Set("userid", Username);
            Preferences.Set("password", Password);
            Preferences.Set("database", Database);
            _ = Application.Current.SavePropertiesAsync();
            UserDialogs.Instance.ShowLoading("Connecting to database...", maskType: MaskType.Black);


            await Task.Run(() =>
            {
                IsShow = Mysqldatabase.tryConnectionAsync();
            }).ConfigureAwait(true);

            if (IsShow)
            {
                mysqlGET mysqlGET = new mysqlGET();
                bool isloggedin = Preferences.Get("islogged", false);


                if (isloggedin)
                {
                    userloginProfileModel userloginProfile = mysqlGET.mysqlgetloggedUserInfo();
                    if (userloginProfile.Branchid == 21)
                        Application.Current.MainPage = new CommiOrderPage();
                    else
                        Application.Current.MainPage = new BranchOrderPage();
                }
                else
                    Application.Current.MainPage = new NavigationPage(new LoginPage());

            }
            else
                IsShow = true;
            UserDialogs.Instance.HideLoading();
        }


    }
}
