using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MT.Models;
using MT.Services;
using System.Collections.Generic;
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

        private mysqlGET mysqlget;

        public registerpageviewmodel()
        {
            mysqlget = new mysqlGET();


            loadBranchItemsAsync();
        }

        async void loadBranchItemsAsync()
        {
            mysqldatabase mysqldatabase = new mysqldatabase();

            //get BRANCH TABLE FROM DATABASE TO USE INTERNALLY FOR LOW QUEUE TIMES
            UserDialogs.Instance.ShowLoading("Retrieving data from commissary");
            await mysqldatabase.loadbranchandproducts();

            //SAVES THE QUEUED BRANCH DATA FROM DATABASE
            loadedProfileModel loadedProfile = mysqldatabase.getBranchandproducts();

            //LOAD THE BRANCHES SAVED 
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
