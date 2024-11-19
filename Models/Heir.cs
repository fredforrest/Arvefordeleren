using System.Data;

namespace Arvefordeleren.Models
{
    public class Heir
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RelationType Relation { get; set; }

    }
    public enum RelationType
    {
       Barn,
       Barnebarn,
       Forældre,
       Bedsteforældre,
       Andet
    }

}
