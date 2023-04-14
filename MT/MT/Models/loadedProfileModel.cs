using System.Collections.Generic;

namespace MT.Models
{
    public class loadedProfileModel
    {
        public List<branchProfileModel> BranchProfiles { get; set; }
        public List<productProfileModel> productProfileModels { get; set; }

        public loadedProfileModel() {
            BranchProfiles = new List<branchProfileModel>();
            productProfileModels = new List<productProfileModel>();
        }
    }
}
