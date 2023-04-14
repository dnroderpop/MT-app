using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Services;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MT.ViewModels
{
    public partial class noconnectionpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        public string server;
        [ObservableProperty]
        public string port;
        [ObservableProperty]
        public string username;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public string database;

        mysqldatabase Mysqldatabase;

        public noconnectionpageviewmodel()
        {
            Mysqldatabase = new mysqldatabase();

            Server = Preferences.Get("server", "122.54.146.208");
            Port = Preferences.Get("port", "3306");
            Username = Preferences.Get("userid", "rodericks");
            Password = Preferences.Get("password", "mtchoco");
            Database = Preferences.Get("database", "mangtinapay");

            tryConnectionAsync();
        }

        [RelayCommand]
        async void tryConnectionAsync()
        {
            Preferences.Set("server", Server);
            Preferences.Set("port", Port);
            Preferences.Set("userid", Username);
            Preferences.Set("password", Password);
            Preferences.Set("database", Database);
            await Xamarin.Forms.Application.Current.SavePropertiesAsync();

            UserDialogs.Instance.ShowLoading("Connecting to database...", maskType: MaskType.Black);
            await Mysqldatabase.tryConnectionAsync(Server, Username, Password, Database, uint.Parse(Port));
        }


    }
}
