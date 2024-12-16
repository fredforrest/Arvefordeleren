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
            //PROBLEMER HERRRRRR MED SHARE
        }
        private static void UpdateShares()
        {
            var children = ForcedHeirs.Where(h => h.Relation == RelationType.Barn).ToList();
            var spouse = ForcedHeirs.FirstOrDefault(h => h is Testator testator && testator.Address != null);
            var parents = ForcedHeirs.Where(h => h.Relation == RelationType.Forældre).ToList();

            // Hvis der ikke er børn eller forældre
            if (children.Count == 0 && parents.Count == 0)
            {
                // Hvis der er ægtefælle, får ægtefællen 100%
                if (spouse != null)
                {
                    spouse.Share = 100.0;
                }
            }
            else
            {
                // Hvis der er børn
                if (children.Count > 0)
                {
                    if (spouse != null)
                    {
                        // Hvis der er ægtefælle, får ægtefællen 50%
                        spouse.Share = 50.0;

                        // Fordel resterende 50% til børnene
                        double equalShare = 50.0 / children.Count;
                        foreach (var child in children)
                        {
                            child.Share = equalShare;
                        }
                    }
                    else
                    {
                        // Hvis der ikke er ægtefælle, og der er børn, fordeles 100% mellem børnene
                        double equalShare = 100.0 / children.Count;
                        foreach (var child in children)
                        {
                            child.Share = equalShare;
                        }
                    }
                }

                // Hvis der er forældre, men ikke børn
                else if (parents.Count > 0)
                {
                    if (spouse != null)
                    {
                        // Hvis der er ægtefælle, får ægtefællen 50%
                        spouse.Share = 50.0;

                        // Fordel resterende 50% til forældrene
                        double equalShareForParents = 50.0 / parents.Count;

                        foreach (var parent in parents)
                        {
                            parent.Share = equalShareForParents;
                        }
                    }
                    else
                    {
                        // Hvis der ikke er ægtefælle, og der kun er forældre, deles 100% mellem forældrene
                        double equalShareForParents = 100.0 / parents.Count;
                        foreach (var parent in parents)
                        {
                            parent.Share = equalShareForParents;
                        }
                    }
                }
            }

            // Hvis der er både ægtefælle og børn, forældre får ikke noget
            if (spouse != null && children.Count > 0)
            {
                // Forældre får intet, deres share sættes til 0
                foreach (var parent in parents)
                {
                    parent.Share = 0.0;
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

            if (IsForcedRelation((RelationType)heir.Relation))
            {
                ForcedHeirs.Add(new Person
                {
                    Id = heir.Id,
                    Name = heir.Name,
                    Relation = heir.Relation
                });

                UpdateShares();
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

    }
}


