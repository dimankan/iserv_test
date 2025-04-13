public interface IUniversitiesService
{
    Task<List<University>> GetUniversities(string country = null, string name = null);
}