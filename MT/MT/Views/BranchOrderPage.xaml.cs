using MT.ViewModels;
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

            (BindingContext as branchorderpageviewmodel)?.OnAppearing();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as branchorderpageviewmodel)?.OnAppearing();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as branchorderpageviewmodel)?.OnAppearing();
        }
    }
}