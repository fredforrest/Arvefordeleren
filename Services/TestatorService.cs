using Arvefordeleren.Models;
using Arvefordeleren.Models.Repositories;

namespace Arvefordeleren.Services
{
    public class TestatorService
    {
        public void EstablishRelationToHeir(Heir heir, int selectedTestatorId)
        {
            if (heir.TypeOfChild == TypeOfChild.Fællesbarn)
            {
                foreach (var testator in TestatorRepository.testators)
                {
                    if (!testator.Heirs.Any(h => h.Name == heir.Name && h.Relation == heir.Relation))
                    {
                        testator.Heirs.Add(heir);
                    }
                }
            }
            else if (selectedTestatorId != 0)
            {
                var oldTestator = TestatorRepository.testators.FirstOrDefault(t => t.Heirs.Any(h => h.Name == heir.Name && h.Relation == heir.Relation));

                if (oldTestator != null)
                {
                    var heirToRemove = oldTestator.Heirs.FirstOrDefault(h => h.Name == heir.Name && h.Relation == heir.Relation);
                    oldTestator.Heirs.Remove(heirToRemove);
                }

                var newTestator = TestatorRepository.testators.FirstOrDefault(t => t.Id == selectedTestatorId);

                if (newTestator != null && !newTestator.Heirs.Any(h => h.Name == heir.Name && h.Relation == heir.Relation))
                {
                    newTestator.Heirs.Add(heir);
                }
            }
        }
    }
}
