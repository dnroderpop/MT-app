using Android.Widget;
using MT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommiEditOrderPage : ContentPage
    {

        public CommiEditOrderPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as commieditorderviewmodel)?.OnAppearing();
        }
    }
}