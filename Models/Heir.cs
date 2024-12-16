using System.ComponentModel.DataAnnotations;
using Arvefordeleren.Components.Pages;
using Arvefordeleren.Models.Repositories;
using System.Data;
using static MudBlazor.Icons.Material;

namespace Arvefordeleren.Models
{
    public class Heir : Person
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Relation skal udfyldes!")]
        public RelationType? RelationType { get; set; }
        public double Share { get; set; } = 1.0;
        public int? ParentId { get; set; } // Tilføjet den her da man åbenbart ikke bare kan kalde på relationstypen
        public List<Testator>? Testators { get; set; } = new List<Testator>();

        public string Icon
        {
            get
            {
                switch (RelationType)
                {
                    case Models.RelationType.Barn:
                        return "/images/Barn.png";

                    case Models.RelationType.Barnebarn:
                        return "/images/Barnebarn.png";

                    case Models.RelationType.Forældre:
                        return "/images/Forældre.png";
                    default:
                        return "/images/Andet.png";
                }
            }
        }
    }


    public enum RelationType
    {
       Barn, Barnebarn, Søskende, Forældre, Andet
    }
}
