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
                    //OnForcedHeirsUpdated?.Invoke();
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
                    UpdateShares();
                    //OnForcedHeirsUpdated?.Invoke();
                }
            }
            OnForcedHeirsUpdated?.Invoke();
        }

        private static void UpdateShares()
        {
            if (ForcedHeirs.Count == 1) return;

            // Calculate equal share
            double equalShare = 100.0 / ForcedHeirs.Count;

            // Assign the share to each person in the list
            foreach (var person in ForcedHeirs)
            {
                person.Share = equalShare;
            }

            // Trigger UI update
            OnForcedHeirsUpdated?.Invoke();
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

        // LAV MED "ForcedHeir"/"Person" Where "Adress !=Null


        //public static void DistributeSharesEqually()
        //{
        //    if (ForcedHeirs.Count == 0) return;

        //    double equalShare = 100.0 / ForcedHeirs.Count;
        //    foreach (var heir in ForcedHeirs)
        //    {
        //    heir.Share = equalShare;
        //    }

        //OnForcedHeirsUpdated?.Invoke();

        //}


        //public static void UpdateSharesBasedOnRelation(Testator testator)
        //{
        //    if (testator.Id == 2)
        //    { 
        //        // Spouse er første Heir med Id = 2 
        //        var spouse = TestatorRepository.testators.FirstOrDefault(t => t.Id == 2);

        //        if (spouse == null)
        //        {
        //            throw new InvalidOperationException("Testator er gift, men der er ingen arving registreret som ægtefælle.");
        //        }

        //        var children = ForcedHeirs.Where(h => h.Relation == RelationType.Barn).ToList();

        //        if (children.Count == 0)
        //        {
        //            // Ingen børn - hele charten går til ægtefællen

        //            spouse.Share = 100;
        //            //foreach (var heir in ForcedHeirs.Where(h => h.Id != spouse.Id))
        //            //{
        //            //    heir.Share = 0;
        //            //}
        //        }
        //        else
        //        {
        //            // Børn er til stede - fordel mellem ægtefælle og børn
        //            DistributeSharesWithSpouseAndChildren((Testator)spouse, children);
        //        }

        //        OnForcedHeirsUpdated?.Invoke(); // Opdater UI
        //    }
        //    else
        //    {
        //        // Testator er ikke gift - del ligeligt mellem arvinger
        //        DistributeSharesEqually();
        //    }
        //}

        //private static void DistributeSharesWithSpouseAndChildren(Testator spouse, List<Person> children)
        //{
        //    throw new NotImplementedException();
        //}

        //public static void DistributeSharesWithSpouseAndChildren(Person spouse, List<Person> children)
        //{


        //    if (children.Count == 1 || spouse.Address != null)
        //    {
        //        // Én arving - 50% til ægtefællen, 50% til barnet
        //        spouse.Share = 50;
        //        children[0].Share = 50;
        //    }
        //    else
        //    {
        //        // Flere arvinger - 50% til ægtefællen, resten fordeles ligeligt mellem børnene
        //        spouse.Share = 50;
        //        double childShare = 50.0 / children.Count;

        //        foreach (var child in children)
        //        {
        //            child.Share = childShare;
        //        }
        //    }

        //    foreach (var heir in ForcedHeirs.Where(h => h.Id != spouse.Id && !children.Contains(h)))
        //    {
        //        heir.Share = 0;
        //    }

        public static void DistributeSharesWithSpouseAndChildren()
        {

            foreach (var person in ForcedHeirs)
            {
                person.Share = 100.0;
            }

            // Identify the spouse in the ForcedHeirs list (where Address is NOT NULL)
            var spouse = ForcedHeirs.FirstOrDefault(person => person.Address != null);

            if (spouse == null)
            {
                throw new InvalidOperationException("No spouse found in the ForcedHeirs list.");
            }

            // Identify children in the ForcedHeirs list (Relation == Barn)
            var children = ForcedHeirs.Where(person => person.Relation == RelationType.Barn).ToList();

            // Allocate 50% share to the spouse
            spouse.Share = 50;

            if (children.Count == 0)
            {
                // No children, spouse gets the entire 100% share
                foreach (var person in ForcedHeirs.Where(p => p.Id != spouse.Id))
                {
                    person.Share = 0;
                }
            }
            else
            {
                // Divide the remaining 50% equally among the children
                double childShare = 50.0 / children.Count;

                foreach (var child in children)
                {
                    child.Share = childShare;
                }

                // Set share to 0 for all others who are neither spouse nor children
                foreach (var person in ForcedHeirs.Where(p => p.Id != spouse.Id && !children.Contains(p)))
                {
                    person.Share = 0;
                }
            }

            // Trigger UI update
            OnForcedHeirsUpdated?.Invoke();
        }

    }
}


