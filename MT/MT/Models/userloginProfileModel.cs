
using CommunityToolkit.Mvvm.ComponentModel;

namespace MT.Models
{
    public partial class userloginProfileModel : ObservableObject
    {
        [ObservableProperty]
        int id;
        [ObservableProperty]
        string fullname;
        [ObservableProperty]
        int branchid;
        [ObservableProperty]
        string branchname;

        public userloginProfileModel()
        {

        }

        public userloginProfileModel(int ID, string FULLNAME, int BRANCHID, string BRANCHNAME)
        {
            Id = ID;
            Fullname = FULLNAME;
            Branchid = BRANCHID;
            Branchname = BRANCHNAME;
        }
    }
}
