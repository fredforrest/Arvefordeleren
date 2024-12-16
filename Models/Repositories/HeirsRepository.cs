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
        public static void UpdateShares()
        {
            var children = ForcedHeirs.Where(h => h.Relation == RelationType.Barn).ToList();
            var spouse = ForcedHeirs.FirstOrDefault(h => h is Testator testator && testator.Address != null);
            var parents = ForcedHeirs.Where(h => h.Relation == RelationType.For�ldre).ToList();

            // Hvis der ikke er b�rn eller for�ldre
            if (children.Count == 0 && parents.Count == 0)
            {
                // Hvis der er �gtef�lle, f�r �gtef�llen 100%
                if (spouse != null)
                {
                    spouse.Share = 100.0;
                }
            }
            else
            {
                // Hvis der er b�rn
                if (children.Count > 0)
                {
                    if (spouse != null)
                    {
                        // Hvis der er �gtef�lle, f�r �gtef�llen 50%
                        spouse.Share = 50.0;

                        // Fordel resterende 50% til b�rnene
                        double equalShare = 50.0 / children.Count;
                        foreach (var child in children)
                        {
                            child.Share = equalShare;
                        }
                    }
                    else
                    {
                        // Hvis der ikke er �gtef�lle, og der er b�rn, fordeles 100% mellem b�rnene
                        double equalShare = 100.0 / children.Count;
                        foreach (var child in children)
                        {
                            child.Share = equalShare;
                        }
                    }
                }

                // Hvis der er for�ldre, men ikke b�rn
                else if (parents.Count > 0)
                {
                    if (spouse != null)
                    {
                        // Hvis der er �gtef�lle, f�r �gtef�llen 50%
                        spouse.Share = 50.0;

                        // Fordel resterende 50% til for�ldrene
                        double equalShareForParents = 50.0 / parents.Count;

                        foreach (var parent in parents)
                        {
                            parent.Share = equalShareForParents;
                        }
                    }
                    else
                    {
                        // Hvis der ikke er �gtef�lle, og der kun er for�ldre, deles 100% mellem for�ldrene
                        double equalShareForParents = 100.0 / parents.Count;
                        foreach (var parent in parents)
                        {
                            parent.Share = equalShareForParents;
                        }
                    }
                }
            }

            // Hvis der er b�de �gtef�lle og b�rn, for�ldre f�r ikke noget
            if (spouse != null && children.Count > 0)
            {
                // For�ldre f�r intet, deres share s�ttes til 0
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

            // Udl�s en event for at opdatere UI
            OnForcedHeirsUpdated?.Invoke();
        }

        private static bool IsForcedRelation(RelationType relation)
        {
            return relation == RelationType.Barn ||
                   relation == RelationType.Barnebarn ||
                   relation == RelationType.For�ldre ||
                   relation == RelationType.Bedstefor�ldre;
        }

    }
}


