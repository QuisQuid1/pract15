using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TRPO_pr15.Data;
using TRPO_pr15.Models;

namespace TRPO_pr15.Services;

public class TagService
{
    private readonly AppDbContext _db = BaseDbService.Instance.Context;

    public ObservableCollection<Tag> Tags { get; set; } = new();

    public TagService()
    {
        GetAll();
    }

    public void GetAll()
    {
        var tags = _db.Tags
            .Include(t => t.Products)
            .ToList();

        Tags.Clear();
        foreach (var tag in tags)
            Tags.Add(tag);
    }

    public void Add(Tag tag)
    {
        var toAdd = new Tag { Name = tag.Name };
        _db.Add(toAdd);
        Commit();
        Tags.Add(toAdd);
    }

    public void Remove(Tag tag)
    {
        _db.Remove(tag);
        if (Commit() > 0 && Tags.Contains(tag))
            Tags.Remove(tag);
    }

    public int Commit() => _db.SaveChanges();
}
