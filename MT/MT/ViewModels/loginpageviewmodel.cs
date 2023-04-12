using MT.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MT.ViewModels
{
    public class loginpageviewmodel : INotifyPropertyChanged
    {
        string username = "", password = "";
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnpropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnpropertyChanged();
            }
        }
        ICommand loginButtonCommand;

        public ICommand LoginButtonCommand => loginButtonCommand ?? new Command(submit);

        void submit()
        {
            //App.Current.MainPage.DisplayActionSheet(username,password,"Okay");

            if (username == "1")
                _ = App.Current.MainPage.Navigation.PushAsync(new BranchOrderPage(), true);
            else
                _ = App.Current.MainPage.Navigation.PushAsync(new CommiOrderPage(), true);
        }

        void OnpropertyChanged([CallerMemberName]string propertyName = "")=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
    }
}
