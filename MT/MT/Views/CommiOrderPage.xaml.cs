using MT.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommiOrderPage : ContentPage
    {
        public CommiOrderPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as commiorderpageviewmodel)?.OnAppearing();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as commiorderpageviewmodel)?.OnAppearing();
        }
    }
}