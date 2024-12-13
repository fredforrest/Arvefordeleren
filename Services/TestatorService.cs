using Arvefordeleren.Models;
using Arvefordeleren.Models.Repositories;

namespace Arvefordeleren.Services
{
    public class TestatorService
    {
        public void EstablishRelationToHeir(Heir heir, int? selectedTestatorId)
        {
            if (selectedTestatorId.HasValue)
            {
                var newTestator = TestatorRepository.testators.FirstOrDefault(t => t.Id == selectedTestatorId);

                if (newTestator != null && !newTestator.Heirs.Any(h => h.Id == heir.Id))
                {
                    newTestator.Heirs.Add(heir);
                }
            }
        }
    }
}
