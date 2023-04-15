using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MT.ViewModels
{
    public partial class branchorderpageviewmodel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy = false;

        public void branchoderpageviewmodel()
        {

        }

        [RelayCommand]
        void onPulltoRefresh()
        {
            IsBusy = true;

            Task.Delay(2000);

            IsBusy = false;
        }


    }
}
