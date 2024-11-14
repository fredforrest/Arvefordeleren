
namespace Arvefordeleren.Models.Repositories
{
    public class ArvingerRepository
    {
        private static List<Arving> _arvinger = new List<Arving>();

        public static void AddArving(Arving arving)
        {
            _arvinger.Add(arving);
        }

        public static void RemoveArving(Arving arving)
        {
            _arvinger.Remove(arving);
        }

        public static List<Arving> GetArvinger()
        {
            return _arvinger;
        }
    }
}