using System.ComponentModel.DataAnnotations;
using Arvefordeleren.Components.Pages;
using Arvefordeleren.Models.Repositories;
using System.Data;
using static MudBlazor.Icons.Material;

namespace Arvefordeleren.Models
{
    public class Heir
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Navn skal udfyldes!")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Relation skal udfyldes!")]
        public RelationType Relation { get; set; }
        public double Share { get; set; } = 1.0;
        public int? ParentId { get; set; } // Tilføjet den her da man åbenbart ikke bare kan kalde på relationstypen
        public List<Heir> Children { get; set; } = new List<Heir>(); // Tilføjet den her da man åbenbart ikke bare kan kalde på relationstypen


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

}
