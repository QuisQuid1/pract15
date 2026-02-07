using System.Collections.ObjectModel;

namespace TRPO_pr15.Models;

public partial class Brand : ObservableObject
{
    private int _id;
    private string _name = string.Empty;
    private ObservableCollection<Product> _products = new();

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

    public ObservableCollection<Product> Products
    {
        get => _products;
        set => SetProperty(ref _products, value ?? new ObservableCollection<Product>());
    }
}
