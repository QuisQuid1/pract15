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
    /// Логика взаимодействия для BrandPage.xaml
    /// </summary>
    public partial class BrandPage : Page 
    {
        public Brand SelectedBrand { get; set; } = new();
        public Brand TextBoxBrand { get; set; } = new();
        public BrandService _brandService { get; set; } = MainWindow.BrandService;
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

        public BrandPage()
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
            _brandService.Add(TextBoxBrand);
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SelectedBrand == null)
            {
                MessageBox.Show("Выберите бренд!");
                return;
            }
            if (SelectedBrand.Products.Count > 0)
            {
                MessageBox.Show("Нельзя удалить бренд, так как он используется продуктом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Вы действительно хотите удалить бренд?", "Удалить?",
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _brandService.Remove(SelectedBrand);
                SelectedBrand = new();
                TextBoxBrand.Name = String.Empty;
            }
        }
        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SelectedBrand == null)
            {
                MessageBox.Show("Выберите бренд!");
                return;
            }
            if (!ValidateForm())
            {
                MessageBox.Show("Заполнены не все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SelectedBrand.Name = TextBoxBrand.Name;
            _brandService.Commit();
            TextBoxBrand.Name = String.Empty;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); 
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedBrand != null)
                TextBoxBrand.Name = SelectedBrand.Name;
        }
    }
}
