namespace Arvefordeleren.Models.Repositories
{
    public static class TestatorRepository
    {
        private static List<Testator> testators = new List<Testator>();
        


        public static void AddNewTestator(Testator testator)
        {
            var maxId = testators.Max(t => t.Id);
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

        public static List<Testator> GetTestators() => new List<Testator>(testators);




    }
}
