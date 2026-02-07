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
    /// Логика взаимодействия для TagPage.xaml
    /// </summary>
    public partial class TagPage : Page
    {
        public TagService _tagService { get; set; } = MainWindow.TagService;
        public ProductService _productService { get; set; } = MainWindow.ProductService; 
        public ProductTagService _productTagService { get; set; } = MainWindow.ProductTagService;
        public Tag SelectedTag { get; set; } = new();
        public Product SelectedProduct { get; set; } = new();
        public Tag TextBoxTag { get; set; } = new();
        void UpdateValidation()
        {
            Name.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        bool ValidateForm()
        {
            UpdateValidation();

            bool hasError = Validation.GetHasError(Name);

            return !hasError;
        }

        public TagPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Заполнены не все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _tagService.Add(TextBoxTag);
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SelectedTag == null)
            {
                MessageBox.Show("Выберите тег!");
                return;
            }
            if (SelectedTag.Products.Count > 0)
            {
                MessageBox.Show("Нельзя удалить тег, так как он используется продуктом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите удалить тег?", "Удалить?",
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _tagService.Remove(SelectedTag);
                SelectedTag = new();
                TextBoxTag.Name = String.Empty;
            }
        }
        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            if (SelectedTag == null)
            {
                MessageBox.Show("Выберите тег!");
                return;
            }
            if (!ValidateForm())
            {
                MessageBox.Show("Заполнены не все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SelectedTag.Name = TextBoxTag.Name;
            _tagService.Commit();
            TextBoxTag.Name = String.Empty;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedTag != null)
                TextBoxTag.Name = SelectedTag.Name;
        }

        private void AddProductTagButton_Click(object sender, RoutedEventArgs e)
        {
            _productTagService.AddTagToProduct(SelectedProduct.Id, SelectedTag.Id);
        }
        private void RemoveProductTagButton_Click(object sender, RoutedEventArgs e)
        {
            _productTagService.RemoveTagFromProduct(SelectedProduct.Id, SelectedTag.Id);
        }
    }
}
