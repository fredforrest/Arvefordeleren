namespace Arvefordeleren.Models
{
    public class Heir
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HeirType Type { get; set; }
    }

    public enum HeirType
    {
        Friarv,
        Tvangsarv
    }
}
