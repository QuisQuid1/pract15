using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TRPO_pr15.Pages;
using TRPO_pr15.Services;

namespace TRPO_pr15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static ProductService _productService = new();
        private static BrandService _brandService = new();
        private static CategoryService _categoryService = new();
        private static TagService _tagService = new();
        private static ProductTagService _productTagService = new();
        private string _windowTitle = "Товары магазина";
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            MainFrame.Navigate(new LoginPage());
        }
        public static ProductService ProductService => _productService;
        public static BrandService BrandService => _brandService;
        public static CategoryService CategoryService => _categoryService;
        public static TagService TagService => _tagService;
        public static ProductTagService ProductTagService => _productTagService;

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page)
                WindowTitle = $"{page.Title}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName =
        null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}