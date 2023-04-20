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
    public partial class BranchOrderPage : ContentPage
    {
        public BranchOrderPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            branchorderpageviewmodel binder = (branchorderpageviewmodel)this.BindingContext;

            binder.onPulltoRefresh();
        }

        protected void refreshed()
        {
            ;
        }
    }
}