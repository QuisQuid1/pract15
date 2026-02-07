using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TRPO_pr15.Data;
using TRPO_pr15.Models;

namespace TRPO_pr15.Services;

public class CategoryService
{
    private readonly AppDbContext _db = BaseDbService.Instance.Context;

    public ObservableCollection<Category> Categories { get; set; } = new();

    public CategoryService()
    {
        GetAll();
    }

    public void GetAll()
    {
        var categories = _db.Categories
            .Include(c => c.Products)
            .ToList();

        Categories.Clear();
        foreach (var category in categories)
            Categories.Add(category);
    }

    public void Add(Category category)
    {
        var toAdd = new Category { Name = category.Name };
        _db.Add(toAdd);
        Commit();
        Categories.Add(toAdd);
    }

    public void Remove(Category category)
    {
        _db.Remove(category);
        if (Commit() > 0 && Categories.Contains(category))
            Categories.Remove(category);
    }

    public int Commit() => _db.SaveChanges();
}
