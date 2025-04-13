using Microsoft.EntityFrameworkCore;

public class UniversitiesService : IUniversitiesService
{
    private readonly UniversitiesContext _db;

    public UniversitiesService(UniversitiesContext db)
    {
        _db = db;
    }

    public async Task<(List<University> Items, int TotalCount)> GetUniversities(
      string country = null,
      string name = null,
      int page = 1,
      int pageSize = 20)
    {
        var query = _db.Universities.AsQueryable();

        if (!string.IsNullOrEmpty(country))
        {
            // Альтернатива ILike для любой СУБД
            query = query.Where(u => u.Country.ToLower().Contains(country.ToLower()));
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => u.Name.ToLower().Contains(name.ToLower()));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(u => u.Country)
            .ThenBy(u => u.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}