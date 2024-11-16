namespace Arvefordeleren.Models
{
    public class Arving
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public ArvingType Type { get; set; }
    }

    public enum ArvingType
    {
        Friarv,
        Tvangsarv
    }
}
