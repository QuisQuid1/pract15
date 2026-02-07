using System;
using System.Windows;
using System.Windows.Controls;
using TRPO_pr15.Models;
using TRPO_pr15.Services;

namespace TRPO_pr15.Pages;

/// <summary>
/// Логика взаимодействия для ProductPage.xaml
/// </summary>
public partial class ProductPage : Page
{
    public Product _product { get; set; } = new();
    public ProductService _productService { get; set; } = MainWindow.ProductService;
    public BrandService _brandService { get; set; } = MainWindow.BrandService;
    public CategoryService _categoryService { get; set; } = MainWindow.CategoryService;

    private bool _isEdit = false;

    public ProductPage(Product? product = null)
    {
        InitializeComponent();

        DatePicker.DisplayDateEnd = DateTime.Today;

        if (product != null)
        {
            _product = product;
            _isEdit = true;
            brands.SelectedItem = _product.Brand;
            categories.SelectedItem = _product.Category;
        }

        DataContext = this;
    }

    private void UpdateValidation()
    {
        Name.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        Price.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        Stock.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        Rating.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        Description.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

        brands.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
        categories.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
        DatePicker.GetBindingExpression(DatePicker.SelectedDateProperty)?.UpdateSource();
    }

    private bool ValidateForm()
    {
        UpdateValidation();

        bool hasError =
            Validation.GetHasError(Name) ||
            Validation.GetHasError(Price) ||
            Validation.GetHasError(Stock) ||
            Validation.GetHasError(Rating) ||
            Validation.GetHasError(Description) ||
            Validation.GetHasError(brands) ||
            Validation.GetHasError(categories) ||
            Validation.GetHasError(DatePicker);

        return !hasError;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.GoBack();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateForm())
        {
            MessageBox.Show("Заполнены не все поля или есть ошибки ввода", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // синхронизируем FK из выбранных навигаций (на случай добавления новой записи)
        if (_product.Brand != null)
            _product.BrandId = _product.Brand.Id;
        if (_product.Category != null)
            _product.CategoryId = _product.Category.Id;

        if (_isEdit)
            _productService.Commit();
        else
            _productService.Add(_product);

        NavigationService.GoBack();
    }
}
