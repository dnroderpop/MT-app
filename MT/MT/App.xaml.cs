using MT.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MT
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MainPage = new NavigationPage(new NoConnectionPage());
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
