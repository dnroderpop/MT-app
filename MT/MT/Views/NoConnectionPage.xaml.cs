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
using MT.ViewModels;
using System.Windows.Input;

namespace MT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoConnectionPage : ContentPage
    {
        public NoConnectionPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}