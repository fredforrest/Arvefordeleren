namespace Arvefordeleren.Models.Repositories
{
    public static class TestatorRepository
    {
        private static List<Testator> testators = new List<Testator>();
        


        public static void AddNewTestator(Testator testator)
        {
            
            testators.Add(testator);
        }

        public static Testator GetTestatorByName(string name) => testators.FirstOrDefault(t => t.Name == name);

        public static void DeleteTestator(string name)
        {
            var testator = GetTestatorByName(name);
            if (testator != null)
            {
                testators.Remove(testator);
            }
        }

        public static List<Testator> GetTestators() => new List<Testator>(testators);




    }
}
