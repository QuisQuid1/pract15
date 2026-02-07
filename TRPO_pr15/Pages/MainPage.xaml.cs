using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TRPO_pr15.Models;
using TRPO_pr15.Services;

namespace TRPO_pr15.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ProductService service { get; set; } = MainWindow.ProductService;

        public Product product { get; set; }

        public MainPage(bool isModerator = false)
        {
            InitializeComponent();
            DataContext = this;

            if (!isModerator)
                IsModerator.Visibility = Visibility.Hidden;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PriceLow.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            PriceHigh.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            if (Validation.GetHasError(PriceLow) || Validation.GetHasError(PriceHigh))
                return;
            service.productsView.Refresh();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            service.ResetFilter();
            service.brandName = PriceLow.Text= PriceHigh.Text=CategoryName.Text=BrandName.Text= string.Empty;
            service.productsView.Refresh();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selected && selected.Tag != null)
            {
                service.SortProducts(selected.Tag.ToString()!);
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage());
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (product == null)
            {
                    MessageBox.Show("Выберите товар!");
                    return;
            }  
            
            NavigationService.Navigate(new ProductPage(product));
        }

        private void BrandButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BrandPage());
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryPage());

        }

        private void TagtButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TagPage());
        }

        private void RemoveProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (product == null)
            {
                MessageBox.Show("Товар не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удалить?",
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                service.Remove(product);
            }
        }
    }
}
