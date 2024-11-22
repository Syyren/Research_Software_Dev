using Research_Software_Dev.Models.Studies;

namespace Research_Software_Dev.Data.TempData
{
    public class TempStudies
    {
        public readonly Study study1 = new(
            1, "Test Study 1", "A first test for the purpose of front end design.");
        public readonly Study study2 = new(
            2, "Test Study 2", "A second test for the purpose of front end design.");
        public readonly Study study3 = new(
            3, "Test Study 3", "A third test for the purpose of front end design.");
        public readonly Study study4 = new(
            4, "Test Study 4", "A fourth test for the purpose of front end design.");
    }
}
