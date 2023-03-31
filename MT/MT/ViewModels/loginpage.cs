using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MT.ViewModels
{
    internal class loginpage : BindableObject
    {
        public string username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }


    }
}
