namespace Arvefordeleren.Models.Repositories
{
    public static class TestatorRepository
    {
        public static List<Testator> testators { get; set; } = new List<Testator>
        {
            new Testator { Id = 1 },
            new Testator { Id = 2},
        };

        public static List<Person> ForcedHeirs { get; set; } = new List<Person>();

        public static void AddTestatorToForcedHeirs(Person testator)
        {


            if (testator.Id == 2)
            {
                ForcedHeirs.Add(testator);
            }
        }


        public static void AddNewTestator(Testator testator)
        {
            int maxId = testators.Any() ? testators.Max(t => t.Id) : 0; // Hvis listen er tom, start med ID 1
            testator.Id = maxId + 1;
            testators.Add(testator);
        }

        public static Testator GetTestatorById(int id) => testators.FirstOrDefault(t => t.Id == id);

        public static void DeleteTestator(int id)
        {
            var testator = GetTestatorById(id);
            if (testator != null)
            {
                testators.Remove(testator);
            }
        }
    }
}
