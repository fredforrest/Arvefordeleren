namespace Arvefordeleren.Models.Repositories
{
    public static class ArvingerRepository
    {
        private static readonly List<Arving> _arvinger = new List<Arving>();

        public static void AddArving(Arving arving)
        {
            if (arving != null && !_arvinger.Contains(arving))
            {
                _arvinger.Add(arving);
            }
        }

        public static void RemoveArving(Arving arving)
        {
            if (arving != null && _arvinger.Contains(arving))
            {
                _arvinger.Remove(arving);
            }
        }

        //tester igen igen
        public static List<Arving> GetArvinger()
        {
            return _arvinger;
        }
    }
}
