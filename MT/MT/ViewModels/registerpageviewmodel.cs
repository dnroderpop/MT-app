using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.ViewModels
{
    public partial class registerpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        private string branch;
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string fullname;
        [ObservableProperty]
        private List<string> branchesItems;


        public registerpageviewmodel()
        {
            loadBranchItems();
        }

        void loadBranchItems()
        {

        }


        [RelayCommand]
        void Updatebutton()
        {
            if (Branch != null && Username != null && Password != null && Fullname != null) { }
        }
        
    }
}
