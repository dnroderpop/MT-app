namespace MT.Models
{
    public class branchProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public branchProfileModel() { }
        public branchProfileModel(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
