using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class UniversitiesService : IUniversitiesService
{
    private readonly UniversitiesContext _db;

    public UniversitiesService(UniversitiesContext db)
    {
        _db = db;
    }

    public async Task<List<University>> GetUniversities(string country = null, string name = null)
    {
        var query = _db.Universities.AsQueryable();

        if (!string.IsNullOrEmpty(country))
        {
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
            .ToListAsync();

        return items;
    }
}