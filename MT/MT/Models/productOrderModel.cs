using CommunityToolkit.Mvvm.ComponentModel;
using Java.Sql;
using System;

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
