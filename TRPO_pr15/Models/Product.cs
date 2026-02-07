using System.Collections.ObjectModel;

namespace TRPO_pr15.Models;

public partial class Product : ObservableObject
{
    private int _id;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private decimal _price;
    private int _stock;
    private decimal _rating;
    private DateTime _createdAt = DateTime.Today;
    private int _categoryId;
    private int _brandId;

    private Brand _brand = null!;
    private Category _category = null!;
    private ObservableCollection<Tag> _tags = new();

    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value ?? string.Empty);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value ?? string.Empty);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public int Stock
    {
        get => _stock;
        set => SetProperty(ref _stock, value);
    }

    public decimal Rating
    {
        get => _rating;
        set => SetProperty(ref _rating, value);
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    public int CategoryId
    {
        get => _categoryId;
        set => SetProperty(ref _categoryId, value);
    }

    public int BrandId
    {
        get => _brandId;
        set => SetProperty(ref _brandId, value);
    }

    public Brand Brand
    {
        get => _brand;
        set => SetProperty(ref _brand, value);
    }

    public Category Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public ObservableCollection<Tag> Tags
    {
        get => _tags;
        set => SetProperty(ref _tags, value ?? new ObservableCollection<Tag>());
    }
}
