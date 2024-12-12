using Arvefordeleren.Components.Pages;

namespace Arvefordeleren.Models.Repositories
{
    public static class HeirsRepository
    {
        public static List<Heir> Heirs { get; set; } = new List<Heir>();
        public static List<Person> ForcedHeirs { get; set; } = new List<Person>();

        public static void AddHeir(Heir heir)
        {
            heir.Id = Heirs.Count + 1;
            Heirs.Add(heir);
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
        //public static void AddHeirToFamilyList(Heir heir, Testator testator)
        //{

        //    if (!ForcedHeirs.Any(h => h.Id == heir.Id))
        //    //if(!ForcedHeirs.Any(h => h.Relation == heir.Relation))

        //    {
        //        switch (heir.Relation)
        //        {
        //            case RelationType.Barn:
        //            case RelationType.Barnebarn:
        //            case RelationType.Forældre:
        //            case RelationType.Bedsteforældre:

        //                ForcedHeirs.Add(heir);
        //                OnForcedHeirsUpdated?.Invoke();
        //                break;

        //        }
        //    }
        //}

        //public static void AddHeirToFamilyList(Heir heir, Testator? testator = null)
        //{
        //    if (testator != null)
        //    {
        //        if (!ForcedHeirs.OfType<Testator>().Any(t => t.Id == testator.Id))
        //        {
        //            ForcedHeirs.Add(testator);
        //            OnForcedHeirsUpdated?.Invoke();
        //        }
        //    }
        //    else if (!ForcedHeirs.OfType<Heir>().Any(h => h.Id == heir.Id))
        //    {
        //        if (IsForcedRelation(heir.Relation))
        //        {
        //            ForcedHeirs.Add(heir);
        //            OnForcedHeirsUpdated?.Invoke();
        //        }
        //    }
        //}

        public static void AddHeirToFamilyList(Heir heir, Testator? testator = null)
        {
            if (testator != null)
            {
                if (!ForcedHeirs.OfType<Testator>().Any(t => t.Id == testator.Id))
                {
                    ForcedHeirs.Add(testator);
                    OnForcedHeirsUpdated?.Invoke();
                }
            }
            else if (!ForcedHeirs.Any(h => h.Id == heir.Id))
            {
                if (IsForcedRelation(heir.Relation))
                {
                    ForcedHeirs.Add(new Person
                    {
                        Id = heir.Id,
                        Name = heir.Name,
                        Relation = heir.Relation
                    });
                    OnForcedHeirsUpdated?.Invoke();
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

            if (IsForcedRelation(heir.Relation))
            {
                ForcedHeirs.Add(new Person
                {
                    Id = heir.Id,
                    Name = heir.Name,
                    Relation = heir.Relation
                });
            }

            //else if(!IsForcedRelation(heir.Relation))
            //{
            //    ForcedHeirs.Remove(heir);
            //}

            // Udløs en event for at opdatere UI
            OnForcedHeirsUpdated?.Invoke();
        }

        private static bool IsForcedRelation(RelationType relation)
        {
            return relation == RelationType.Barn ||
                   relation == RelationType.Barnebarn ||
                   relation == RelationType.Forældre ||
                   relation == RelationType.Bedsteforældre;
                   // Muligivis tilføj spouse som ENUM;
        }
    }



}

