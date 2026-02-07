using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using TRPO_pr15.Data;
using TRPO_pr15.Models;

namespace TRPO_pr15.Services;

public class ProductService : ObservableObject
{
    private readonly AppDbContext _db = BaseDbService.Instance.Context;

    public ObservableCollection<Product> Products { get; set; } = new();

    public ICollectionView productsView { get; }
    public string searchQuery { get; set; } = string.Empty;
    public string brandName { get; set; } = string.Empty;
    public string categoryName { get; set; } = string.Empty;
    public string filterPriceFrom { get; set; } = string.Empty;
    public string filterPriceTo { get; set; } = string.Empty;

    public ProductService()
    {
        productsView = CollectionViewSource.GetDefaultView(Products);
        productsView.Filter = FilterProducts;
        GetAll();
    }

    public void GetAll()
    {
        var products = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .ToList();

        Products.Clear();
        foreach (var product in products)
            Products.Add(product);
    }

    public void Add(Product product)
    {
        var brandId = product.Brand?.Id ?? product.BrandId;
        var categoryId = product.Category?.Id ?? product.CategoryId;

        var toAdd = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Rating = product.Rating,
            CreatedAt = product.CreatedAt,
            CategoryId = categoryId,
            BrandId = brandId,
            Brand = product.Brand,
            Category = product.Category
        };

        _db.Add(toAdd);
        Commit();
        Products.Add(toAdd);
    }

    public void Remove(Product product)
    {
        // гарантируем, что продукт отслежен контекстом
        var tracked = _db.Products
            .Include(p => p.Tags)
            .First(p => p.Id == product.Id);

        tracked.Tags.Clear(); // удалит связи (строки ProductTags)
        _db.Products.Remove(tracked);

        if (Commit() > 0 && Products.Contains(product))
            Products.Remove(product);
    }

    public int Commit() => _db.SaveChanges();

    public bool FilterProducts(object obj)
    {
        if (obj is not Product product)
            return false;

        if (!string.IsNullOrWhiteSpace(searchQuery) &&
            !product.Name.Contains(searchQuery, StringComparison.CurrentCultureIgnoreCase))
            return false;

        if (!string.IsNullOrWhiteSpace(brandName) &&
            (product.Brand == null || !product.Brand.Name.Contains(brandName, StringComparison.CurrentCultureIgnoreCase)))
            return false;

        if (!string.IsNullOrWhiteSpace(categoryName) &&
            (product.Category == null || !product.Category.Name.Contains(categoryName, StringComparison.CurrentCultureIgnoreCase)))
            return false;

        if (!string.IsNullOrWhiteSpace(filterPriceFrom))
        {
            if (decimal.TryParse(filterPriceFrom.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var minPrice))
            {
                if (minPrice > product.Price)
                    return false;
            }
        }

        if (!string.IsNullOrWhiteSpace(filterPriceTo))
        {
            if (decimal.TryParse(filterPriceTo.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var maxPrice))
            {
                if (maxPrice < product.Price)
                    return false;
            }
        }

        return true;
    }

    public void SortProducts(string tag)
    {
        productsView.SortDescriptions.Clear();

        switch (tag)
        {
            case "Name":
                productsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                break;
            case "PriceDescending":
                productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
                break;
            case "PriceAscending":
                productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
                break;
            case "StockDescending":
                productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Descending));
                break;
            case "StockAscending":
                productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Ascending));
                break;
        }

        productsView.Refresh();
    }

    public void ResetFilter()
    {
        searchQuery = brandName = categoryName = filterPriceFrom = filterPriceTo = string.Empty;
        productsView.Refresh();
    }
}
