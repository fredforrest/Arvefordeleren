using Arvefordeleren.Components.Pages;

namespace Arvefordeleren.Models.Repositories
{
    public static class HeirsRepository
    {
        public static List<Heir> Heirs { get; set; } = new List<Heir>();

        public static void AddHeir(Heir heir)
        {
            heir.Id = Heirs.Count + 1;
            Heirs.Add(heir);
        }

        //public static void RemoveHeir(int id)
        //{
        //    Heir heir = Heirs.Where(x => x.Id == id).FirstOrDefault();
        //    if (heir != null)
        //    {
        //        Heirs.Remove(heir);
        //    }
        //}
        public static void RemoveHeir(int id)
        {
            Heir heir = Heirs.FirstOrDefault(b => b.Id == id);

            if (heir != null)
            {
                Heirs.Remove(heir);
            }
        }
    }
}
