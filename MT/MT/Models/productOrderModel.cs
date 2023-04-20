using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace MT.Models
{
    public partial class productOrderModel : ObservableObject
    {
        [ObservableProperty]
        int id;
        [ObservableProperty]
        int branchid;
        [ObservableProperty]
        int productid;
        [ObservableProperty]
        DateTime date;
        [ObservableProperty]
        string productName;
        [ObservableProperty]
        string productCategory;
        [ObservableProperty]
        double qty;
        [ObservableProperty]
        double price;
        [ObservableProperty]
        double amount;
        [ObservableProperty]
        double yield;

    }
}
