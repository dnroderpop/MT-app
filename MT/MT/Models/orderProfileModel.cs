using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MT.Models
{
    public partial class orderProfileModel : ObservableObject
    {
        [ObservableProperty]
        int branchid;
        [ObservableProperty]
        string branchname;
        [ObservableProperty]
        int items;
        [ObservableProperty]
        string status;
        [ObservableProperty]
        bool isAble;
        [ObservableProperty]
        double amount;
        [ObservableProperty]
        DateTime date;
    }
}
