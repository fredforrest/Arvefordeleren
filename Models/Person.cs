namespace Arvefordeleren.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Share { get; set; }
        public string? Address { get; set; }
        public RelationType? Relation { get; set; }

        public string? Icon
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
}
