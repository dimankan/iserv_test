public interface IUniversitiesService
{
    Task<(List<University> Items, int TotalCount)> GetUniversities(
        string country = null,
        string name = null,
        int page = 1,
        int pageSize = 20);
}