using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Arvefordeleren.Models
{
    public class Testator
    {
        public int Id { get; set; }

        public string? Name { get; set; } = null;

        public string? Address { get; set; } = null;
                

    }
}
