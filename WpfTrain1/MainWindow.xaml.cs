using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfTrain1
{
    public partial class MainWindow : Window
    {
        private List<Product> _allProducts = new List<Product>();

        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalPages = 1;

        public MainWindow()
        {
            InitializeComponent();
            SeedData();
            LoadPage();
        }

        private void SeedData()
        {
            _allProducts.Clear();

            for (int i = 1; i <= 7; i++)
            {
                _allProducts.Add(new Product
                {
                    Id = i,
                    Name = "Product " + i,
                    Price = i * 10,
                    Category = i % 2 == 0 ? "Food" : "Drink"
                });
            }

            CalculateTotalPages();
        }

        private void CalculateTotalPages()
        {
            _totalPages = (int)Math.Ceiling((double)_allProducts.Count / _pageSize);
            if (_totalPages == 0) _totalPages = 1;
            if (_currentPage > _totalPages) _currentPage = _totalPages;
        }

        private void LoadPage()
        {
            ProductGrid.ItemsSource = _allProducts
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            TxtPage.Text = $"{_currentPage} / {_totalPages}";
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPage();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadPage();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (!int.TryParse(TbId.Text, out int id))
            {
                MessageBox.Show("ID phải là số nguyên. Vui lòng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbName.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbName.Focus();
                return;
            }

            if (!decimal.TryParse(TbPrice.Text, out decimal price))
            {
                MessageBox.Show("Price phải là số. Vui lòng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbPrice.Focus();
                return;
            }

            if (CbCategory.SelectedItem == null)
            {
                MessageBox.Show("Bạn phải chọn Category.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                CbCategory.Focus();
                return;
            }

            _allProducts.Add(new Product
            {
                Id = id,
                Name = TbName.Text.Trim(),
                Price = price,
                Category = ((ComboBoxItem)CbCategory.SelectedItem).Content.ToString()
            });

            CalculateTotalPages();
            LoadPage();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (!(ProductGrid.SelectedItem is Product p))
                return;

            // Validation
            if (!int.TryParse(TbId.Text, out int id))
            {
                MessageBox.Show("ID phải là số nguyên. Vui lòng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TbName.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbName.Focus();
                return;
            }

            if (!decimal.TryParse(TbPrice.Text, out decimal price))
            {
                MessageBox.Show("Price phải là số. Vui lòng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                TbPrice.Focus();
                return;
            }

            if (CbCategory.SelectedItem == null)
            {
                MessageBox.Show("Bạn phải chọn Category.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                CbCategory.Focus();
                return;
            }

            p.Id = id;
            p.Name = TbName.Text.Trim();
            p.Price = price;
            p.Category = ((ComboBoxItem)CbCategory.SelectedItem).Content.ToString();
            LoadPage();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductGrid.SelectedItem is Product p)
            {
                _allProducts.Remove(p);
                CalculateTotalPages();
                LoadPage();
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
