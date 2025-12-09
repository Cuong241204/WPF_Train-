using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfTrain1
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Item> Items = new ObservableCollection<Item>();

        public MainWindow()
        {
            InitializeComponent();

            // Sample data
            Items.Add(new Item { Id = 1, Name = "Cuong", Price = 100, Category = "Food" });
            Items.Add(new Item { Id = 2, Name = "Anh", Price = 20, Category = "Drink" });

            DataGridItems.ItemsSource = Items;
        }

        // Add
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TbId.Text, out int id)) return;
            if (!double.TryParse(TbPrice.Text, out double price)) return;

            string category = (CbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();

            Items.Add(new Item
            {
                Id = id,
                Name = TbName.Text,
                Price = price,
                Category = category
            });
        }

        // Edit
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItems.SelectedItem is Item item)
            {
                if (!int.TryParse(TbId.Text, out int id)) return;
                if (!double.TryParse(TbPrice.Text, out double price)) return;

                item.Id = id;
                item.Name = TbName.Text;
                item.Price = price;

                string category = (CbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();
                item.Category = category;

                DataGridItems.Items.Refresh();
            }
        }

        // Delete
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItems.SelectedItem is Item item)
            {
                Items.Remove(item);
            }
        }
    }

    // Model
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}