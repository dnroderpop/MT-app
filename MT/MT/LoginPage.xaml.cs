using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MT.ViewModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;

namespace MT
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            loginpage loginpage = new loginpage();

            login_username.BindingContext = loginpage;
            login_username.SetBinding(ContentProperty, loginpage.username);
            login_password.SetBinding(ContentProperty, loginpage.password);
        }

        private void Register_tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage(), true);
        }

        private void login_click(object sender, EventArgs e)
        {
            DisplayAlert("Username",login_username.Text + " " + login_password.Text,"Cancel");
        }
    }
}
