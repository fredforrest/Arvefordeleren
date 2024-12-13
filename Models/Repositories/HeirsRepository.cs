using Arvefordeleren.Components.Pages;

namespace Arvefordeleren.Models.Repositories
{
    public static class HeirsRepository
    {
         public static List<Heir> Heirs { get; set; } = new List<Heir>();
        public static List<Heir> ForcedHeirs { get; set; } = new List<Heir>();

        public static void AddHeir(Heir heir)
        {
            heir.Id = Heirs.Count + 1;
            Heirs.Add(heir);
        }

         public static void AddForcedHeir(Heir heir)
        {
            if (!ForcedHeirs.Contains(heir))
            {
                ForcedHeirs.Add(heir);
            }
        }

        public static void RemoveHeir(int id)
        {
            Heir heir = Heirs.FirstOrDefault(b => b.Id == id);

            if (heir != null)
            {
                Heirs.Remove(heir);


                var forcedHeir = ForcedHeirs.FirstOrDefault(h => h.Id == id);
                if (forcedHeir != null)
                {
                    ForcedHeirs.Remove(forcedHeir);
                }

                OnForcedHeirsUpdated?.Invoke();
            }
        }

        public static Heir? GetHeirById(int id)
        {
        Heir? heir = Heirs.FirstOrDefault(h => h.Id == id);

        if (heir == null)
        {
            throw new InvalidOperationException($"Heir med Id {id} blev ikke fundet.");
        }

        return heir;
        }

        public static Action? OnForcedHeirsUpdated { get; set; }
        public static void AddHeirToFamilyList(Heir heir)
{
    if (!ForcedHeirs.Any(h => h.Id == heir.Id))
    {
        switch (heir.Relation)
        {
            case RelationType.Barn:
            case RelationType.Barnebarn:
            case RelationType.Forældre:
            case RelationType.Bedsteforældre:
                ForcedHeirs.Add(heir);
                DistributeSharesEqually(); // Fordel andelene
                break;
        }
    }
}

        public static void UpdateHeirRelation(Heir heir)
        {
            var existingHeir = ForcedHeirs.FirstOrDefault(h => h.Id == heir.Id);
            if (existingHeir != null)
            {
                ForcedHeirs.Remove(existingHeir);
            }

            // Tjek om arvingen kvalificerer sig til ForcedHeirs baseret p� relation
            if (IsForcedRelation(heir.Relation))
            {
                ForcedHeirs.Add(heir);
            }


            else if(!IsForcedRelation(heir.Relation))
            {
                ForcedHeirs.Remove(heir);
            }

            // Udl�s en event for at opdatere UI
            OnForcedHeirsUpdated?.Invoke();
        }

        private static bool IsForcedRelation(RelationType relation)
        {
            return relation == RelationType.Barn ||
                   relation == RelationType.Barnebarn ||
                   relation == RelationType.Forældre ||
                   relation == RelationType.Bedsteforældre;
        }

        
public static void DistributeSharesEqually()
{
    if (ForcedHeirs.Count == 0) return;

    double equalShare = 100.0 / ForcedHeirs.Count;
    foreach (var heir in ForcedHeirs)
    {
        heir.Share = equalShare;
    }

    OnForcedHeirsUpdated?.Invoke();
}

public static void UpdateHeirShare(Heir heir, double newShare)
{
    double totalWithoutCurrent = ForcedHeirs
        .Where(h => h.Id != heir.Id)
        .Sum(h => h.Share);

    // Hvis den samlede værdi med ny andel overstiger 100, begræns det
    if (totalWithoutCurrent + newShare <= 100)
    {
        heir.Share = newShare;
    }
    else
    {
        heir.Share = Math.Max(0, 100 - totalWithoutCurrent);
    }

    OnForcedHeirsUpdated?.Invoke(); // Opdater UI
}



    }


}

