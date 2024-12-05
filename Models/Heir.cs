using System.Data;

namespace Arvefordeleren.Models
{
    public class Heir
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RelationType Relation { get; set; }
        public TypeOfChild TypeOfChild { get; set; }
        public List<Testator> Testators { get; set; } = new List<Testator>();

        public string Icon
        {
            get
            {
                switch (Relation)
                {
                    case RelationType.Barn:
                        return "/images/Barn.png";

                    case RelationType.Barnebarn:
                        return "/images/Barnebarn.png";

                    case RelationType.Forældre:
                        return "/images/Forældre.png";

                    case RelationType.Bedsteforældre:
                        return "/images/Bedsteforældre.png";

                    case RelationType.Andet:
                        return "/images/Andet.png";

                    default:
                        return "/images/favicon.png";
                }
            }
        }
    }


    public enum RelationType
    {
       Barn, Barnebarn, Forældre, Bedsteforældre, Andet
    }

    public enum TypeOfChild
    {
        Særbarn, Fællesbarn, Andet
    }

}
