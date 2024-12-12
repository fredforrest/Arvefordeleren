namespace Arvefordeleren.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Share { get; set; }
        public RelationType? Relation { get; set; }
    }
}
