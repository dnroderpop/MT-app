using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MT.Models
{
    internal partial class orderProfileModel : ObservableObject
    {
        [ObservableProperty]
        int branchid;
        [ObservableProperty]
        string branchname;
        [ObservableProperty]
        double items;
        [ObservableProperty]
        string status;
        [ObservableProperty]
        int able;
        [ObservableProperty]
        double amount;
    }
}
