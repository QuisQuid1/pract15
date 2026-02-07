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
    /// Логика взаимодействия для CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        public Category TextBoxCategory { get; set; } = new();
        public Category SelectedCategory { get; set; } = new();
        public CategoryService _categoryService { get; set; } = MainWindow.CategoryService;
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
        public CategoryPage()
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

            _categoryService.Add(TextBoxCategory);
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Выберите категорию!");
                return;
            }
            if (SelectedCategory.Products.Count > 0)
            {
                MessageBox.Show("Нельзя удалить категорию, так как он используется продуктом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите удалить категорию?", "Удалить?",
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _categoryService.Remove(SelectedCategory);
                SelectedCategory = new();
                TextBoxCategory.Name = String.Empty;
            }
        }
        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Выберите категорию!");
                return;
            }
            if (!ValidateForm())
            {
                MessageBox.Show("Заполнены не все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SelectedCategory.Name = TextBoxCategory.Name;
            _categoryService.Commit();
            TextBoxCategory.Name = String.Empty;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCategory != null)
             TextBoxCategory.Name = SelectedCategory.Name;
        }

    }
}
