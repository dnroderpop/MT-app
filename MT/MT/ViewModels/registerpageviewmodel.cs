using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace MT.ViewModels
{
    public partial class registerpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        private int branch;
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string fullname;
        [ObservableProperty]
        private List<branchProfileModel> branchesItems;
        [ObservableProperty]
        branchProfileModel selectedbranch;


        public registerpageviewmodel()
        {
            loadBranchItems();
        }

        void loadBranchItems()
        {
            mysqldatabase mysqldatabase = new mysqldatabase();
            loadedProfileModel loadedProfile = mysqldatabase.getBranchandproducts();
            List<branchProfileModel> branches = loadedProfile.BranchProfiles;
            BranchesItems = branches;
        }


        [RelayCommand]
        async Task Updatebutton()
        {

            if (Branch != -1 && Username != null && Password != null && Fullname != null)
            {
                Branch = Selectedbranch.Id;
                userProfileModel userProfile = new userProfileModel()
                {
                    fullname = Fullname,
                    branch = Branch,
                    username = Username,
                    password = Password,
                    id = 0
                };
                
                mysqlINSERT mysqlINSERT = new mysqlINSERT();
                await mysqlINSERT.Register(userProfile);
            }
            else
                UserDialogs.Instance.Toast("Please complete the form");

            UserDialogs.Instance.HideLoading();
        }
    }
}
