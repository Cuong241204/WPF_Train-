using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfTrain1
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Item> Items = new ObservableCollection<Item>();
        List<Item> FilteredItems = new List<Item>();   // sau khi search
        List<Item> PagedItems = new List<Item>();      // dữ liệu đang hiển thị theo trang

        int CurrentPage = 1;
        int PageSize = 5;   // mỗi trang 5 dòng

        public MainWindow()
        {
            InitializeComponent();

            // Sample data
            Items.Add(new Item { Id = 1, Name = "Cuong", Price = 100, Category = "Food" });
            Items.Add(new Item { Id = 2, Name = "Anh", Price = 20, Category = "Drink" });
            Items.Add(new Item { Id = 3, Name = "Cong", Price = 50, Category = "Food" });
            Items.Add(new Item { Id = 4, Name = "Long", Price = 70, Category = "Electronics" });
            Items.Add(new Item { Id = 5, Name = "Hai", Price = 80, Category = "Food" });
            Items.Add(new Item { Id = 6, Name = "Tuan", Price = 90, Category = "Other" });

            FilteredItems = Items.ToList();

            LoadPage();
        }


        // ============================
        //       PAGINATION
        // ============================
        private void LoadPage()
        {
            int totalPages = (int)Math.Ceiling((double)FilteredItems.Count / PageSize);
            if (totalPages == 0) totalPages = 1;
            if (CurrentPage > totalPages) CurrentPage = totalPages;
            if (CurrentPage < 1) CurrentPage = 1;

            PagedItems = FilteredItems
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            DataGridItems.ItemsSource = PagedItems;

            TxtPageInfo.Text = $"Page {CurrentPage} / {totalPages}";
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
            LoadPage();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            LoadPage();
        }


        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = TbSearch.Text.Trim().ToLower();

            FilteredItems = Items
                .Where(i => i.Name.ToLower().Contains(keyword)
                         || i.Category.ToLower().Contains(keyword))
                .ToList();

            CurrentPage = 1;
            LoadPage();

            TxtStatus.Text = $"Found {FilteredItems.Count} result(s).";
        }

        private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            TbSearch.Text = "";
            FilteredItems = Items.ToList();
            CurrentPage = 1;
            LoadPage();

            TxtStatus.Text = "Search cleared.";
        }


        // ============================
        //            CRUD
        // ============================
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TbId.Text, out int id))
            {
                MessageBox.Show("ID must be a number.");
                return;
            }
            if (!double.TryParse(TbPrice.Text, out double price))
            {
                MessageBox.Show("Price must be a number.");
                return;
            }

            string category = (CbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();

            var newItem = new Item
            {
                Id = id,
                Name = TbName.Text,
                Price = price,
                Category = category
            };

            Items.Add(newItem);

            FilteredItems = Items.ToList();
            LoadPage();

            TxtStatus.Text = "Item added.";
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItems.SelectedItem is Item item)
            {
                if (!int.TryParse(TbId.Text, out int id))
                {
                    MessageBox.Show("ID must be a number.");
                    return;
                }
                if (!double.TryParse(TbPrice.Text, out double price))
                {
                    MessageBox.Show("Price must be a number.");
                    return;
                }

                item.Id = id;
                item.Name = TbName.Text;
                item.Price = price;
                item.Category = (CbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();

                DataGridItems.Items.Refresh();
                TxtStatus.Text = "Item updated.";
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItems.SelectedItem is Item item)
            {
                Items.Remove(item);

                FilteredItems = Items.ToList();
                LoadPage();

                TxtStatus.Text = "Item deleted.";
            }
        }


        // ============================
        //    FILL FORM WHEN SELECT
        // ============================
        private void DataGridItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItems.SelectedItem is Item item)
            {
                TbId.Text = item.Id.ToString();
                TbName.Text = item.Name;
                TbPrice.Text = item.Price.ToString();

                foreach (ComboBoxItem cb in CbCategory.Items)
                {
                    if (cb.Content.ToString() == item.Category)
                    {
                        CbCategory.SelectedItem = cb;
                        break;
                    }
                }
            }
        }

        private void BtnClearForm_Click(object sender, RoutedEventArgs e)
        {
            TbId.Text = "";
            TbName.Text = "";
            TbPrice.Text = "";
            TbSearch.Text = "";
            CbCategory.SelectedIndex = -1;

            DataGridItems.UnselectAll();
            TxtStatus.Text = "Form cleared.";
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
