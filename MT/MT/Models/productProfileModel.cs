using CommunityToolkit.Mvvm.ComponentModel;

namespace MT.Models
{
    public class productProfileModel : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Srpc { get; set; }
        public double Srpb { get; set; }
        public double Avew { get; set; }
        public double Yielding { get; set; }
        public int Able { get; set; }
        public productProfileModel(int id, string name, string category, double srpc, double srpb, double avew, double yielding)
        {
            Id = id;
            Name = name;
            Category = category;
            Srpc = srpc;
            Srpb = srpb;
            Avew = avew;
            Yielding = yielding;
        }

    }
}
