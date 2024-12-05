using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Arvefordeleren.Models
{
    public class Testator
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Navn skal udfyldes!")]
        public string? Name { get; set; } = null;

        public bool isMarried { get; set; }

        public string? Address { get; set; } = null;

        public string? Email { get; set; }


    }
}
