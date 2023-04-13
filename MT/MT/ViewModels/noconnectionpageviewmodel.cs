using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Services;
using System.Threading.Tasks;

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

            //check for setting availability
            if (!Xamarin.Forms.Application.Current.Properties.ContainsKey("server"))
            {
                Xamarin.Forms.Application.Current.Properties["server"] = "122.54.146.208";
                Xamarin.Forms.Application.Current.Properties["port"] = "3306";
                Xamarin.Forms.Application.Current.Properties["userid"] = "rodericks";
                Xamarin.Forms.Application.Current.Properties["password"] = "mtchoco";
                Xamarin.Forms.Application.Current.Properties["database"] = "mangtinapay";
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }

            Server = Xamarin.Forms.Application.Current.Properties["server"].ToString();
            Port = Xamarin.Forms.Application.Current.Properties["port"].ToString();
            Username = Xamarin.Forms.Application.Current.Properties["userid"].ToString();
            Password = Xamarin.Forms.Application.Current.Properties["password"].ToString();
            Database = Xamarin.Forms.Application.Current.Properties["database"].ToString();

            tryConnectionAsync();
        }

        [RelayCommand]
        async void tryConnectionAsync()
        {
            Xamarin.Forms.Application.Current.Properties["server"] = Server;
            Xamarin.Forms.Application.Current.Properties["port"] = Port;
            Xamarin.Forms.Application.Current.Properties["userid"] = Username;
            Xamarin.Forms.Application.Current.Properties["password"] = Password;
            Xamarin.Forms.Application.Current.Properties["database"] = Database;
            await Xamarin.Forms.Application.Current.SavePropertiesAsync();

            UserDialogs.Instance.ShowLoading("Connecting to database...", maskType: MaskType.Black);
            await Mysqldatabase.tryConnectionAsync(Server, Username, Password, Database, uint.Parse(Port));
            UserDialogs.Instance.HideLoading();
        }


    }
}
