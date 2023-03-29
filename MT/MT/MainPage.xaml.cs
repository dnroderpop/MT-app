using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MT
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Register_tapped(object sender, EventArgs e)
        {
            DisplayAlert("Register throw here", "register me sir", "Cancel");
        }

        private void login_click(object sender, EventArgs e)
        {
            DisplayAlert("Username "+ text_userlogin.Text,"Password " + text_passlogin.Text, "Cancel");
        }
    }
}
