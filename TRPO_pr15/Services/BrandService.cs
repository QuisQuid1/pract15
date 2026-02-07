using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TRPO_pr15.Data;
using TRPO_pr15.Models;

namespace TRPO_pr15.Services;

public class BrandService
{
    private readonly AppDbContext _db = BaseDbService.Instance.Context;

    public ObservableCollection<Brand> Brands { get; set; } = new();

    public BrandService()
    {
        GetAll();
    }

    public void GetAll()
    {
        var brands = _db.Brands
            .Include(b => b.Products)
            .ToList();

        Brands.Clear();
        foreach (var brand in brands)
            Brands.Add(brand);
    }

    public void Add(Brand brand)
    {
        var toAdd = new Brand { Name = brand.Name };
        _db.Add(toAdd);
        Commit();
        Brands.Add(toAdd);
    }

    public void Remove(Brand brand)
    {
        _db.Remove(brand);
        if (Commit() > 0 && Brands.Contains(brand))
            Brands.Remove(brand);
    }

    public int Commit() => _db.SaveChanges();
}
