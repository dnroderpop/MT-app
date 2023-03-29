using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;

namespace MT
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Register_tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage(), true);
        }

        private void login_click(object sender, EventArgs e)
        {
            DisplayAlert("Username "+ text_userlogin.Text,"Password " + text_passlogin.Text, "Cancel");
        }
    }
}
