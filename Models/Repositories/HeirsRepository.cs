using Arvefordeleren.Components.Pages;

namespace Arvefordeleren.Models.Repositories
{
    public static class HeirsRepository
    {
        public static List<Heir> Heirs { get; set; } = new List<Heir>();
        public static List<Person> ForcedHeirs => Shared.SharedData.ForcedHeirs;

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

        public static void AddHeirToFamilyList(Heir heir, Testator? testator = null)
        {
            if (testator != null)
            {
                if (!ForcedHeirs.OfType<Testator>().Any(t => t.Id == testator.Id))
                {
                    ForcedHeirs.Add(testator);
                    
                    UpdateShares();
                    OnForcedHeirsUpdated?.Invoke();
                   

                }
            }
            else if (!ForcedHeirs.Any(h => h.Id == heir.Id))
            {
                if (IsForcedRelation((RelationType)heir.Relation))
                {
                    ForcedHeirs.Add(new Person
                    {
                        Id = heir.Id,
                        Name = heir.Name,
                        Relation = heir.Relation
                    });
                    
             
                }
                UpdateShares();
                OnForcedHeirsUpdated?.Invoke();
            }
        }

        private static void UpdateShares()
        {
            if (ForcedHeirs.Count == 0) return;

            // Calculate equal share
            double equalShare = 100.0 / ForcedHeirs.Count;

            // Assign the share to each person in the list
            foreach (var person in ForcedHeirs)
            {
                person.Share = equalShare;
            }
        }




        public static void UpdateHeirRelation(Heir heir)
        {
            var existingHeir = ForcedHeirs.FirstOrDefault(h => h.Id == heir.Id);
            if (existingHeir != null)
            {
                ForcedHeirs.Remove(existingHeir);
            }

            if (IsForcedRelation((RelationType)heir.Relation))
            {
                ForcedHeirs.Add(new Person
                {
                    Id = heir.Id,
                    Name = heir.Name,
                    Relation = heir.Relation
                });
            }

            // Udløs en event for at opdatere UI
            OnForcedHeirsUpdated?.Invoke();
        }

        private static bool IsForcedRelation(RelationType relation)
        {
            return relation == RelationType.Barn ||
                   relation == RelationType.Barnebarn ||
                   relation == RelationType.Forældre ||
                   relation == RelationType.Bedsteforældre;
        }


        public static void DistributeSharesWithSpouseAndChildren()
        {

            foreach (var person in ForcedHeirs)
            {
                person.Share = 100.0;
            }

            // Identificer spouse i ForcedHeirs list (hvor Address is NOT NULL)
            var spouse = ForcedHeirs.FirstOrDefault(person => person.Address != null);

            if (spouse == null)
            {
                throw new InvalidOperationException("No spouse found in the ForcedHeirs list.");
            }

            // Identificer children i ForcedHeirs list (Relation == Barn)
            var children = ForcedHeirs.Where(person => person.Relation == RelationType.Barn).ToList();

            // Giv 50% share til spouse
            spouse.Share = 50;

            if (children.Count == 0)
            {
                spouse.Share = 100;

                // Ingen børn, spouse får 100%
                //foreach (var person in ForcedHeirs.Where(p => p.Id != spouse.Id))
                //{
                //    person.Share = 0;
                //}
            }
            else
            {
                spouse.Share = 50;

                double childShare = 50.0 / children.Count;
                foreach (var child in children)

                {
                    child.Share = childShare;
                }


                //// Divider remaining 50% lige mellem børn
                //double childShare = 50.0 / children.Count;

                //foreach (var child in children)
                //{
                //    child.Share = childShare;
                //}
            }

            // Trigger UI update
            OnForcedHeirsUpdated?.Invoke();
        }

    }
}


