using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfTrain1
{
    public partial class MainWindow : Window
    {
        public class SanPham : INotifyPropertyChanged, IDataErrorInfo
        {
            private int _ma;
            private string _ten;
            private int _gia;

            public int Ma
            {
                get => _ma;
                set { _ma = value; OnPropertyChanged(nameof(Ma)); }
            }

            public string Ten
            {
                get => _ten;
                set { _ten = value; OnPropertyChanged(nameof(Ten)); }
            }

            public int Gia
            {
                get => _gia;
                set { _gia = value; OnPropertyChanged(nameof(Gia)); }
            }

            public string Error => null;

            public string this[string columnName]
            {
                get
                {
                    switch (columnName)
                    {
                        case nameof(Ma):
                            if (Ma <= 0) return "Mã SP phải > 0";
                            break;
                        case nameof(Ten):
                            if (string.IsNullOrWhiteSpace(Ten)) return "Tên SP không được để trống";
                            break;
                        case nameof(Gia):
                            if (Gia <= 0) return "Giá phải > 0";
                            break;
                    }
                    return null;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string prop) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<SanPham> _all = new ObservableCollection<SanPham>();
        private ObservableCollection<SanPham> _page = new ObservableCollection<SanPham>();
        private SanPham _current = new SanPham();

        private int _pageSize = 5;
        private int _pageIndex = 1;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _current;
            dgSanPham.ItemsSource = _page;
            LoadPage();
        }

        private void LoadPage()
        {
            _page.Clear();
            var items = _all.Skip((_pageIndex - 1) * _pageSize).Take(_pageSize);
            foreach (var i in items) _page.Add(i);

            int totalPage = (_all.Count + _pageSize - 1) / _pageSize;
            txtPage.Text = $"Trang {_pageIndex}/{(totalPage == 0 ? 1 : totalPage)}";
        }

        private bool IsValidInput()
        {
            return string.IsNullOrEmpty(_current[nameof(SanPham.Ma)]) &&
                   string.IsNullOrEmpty(_current[nameof(SanPham.Ten)]) &&
                   string.IsNullOrEmpty(_current[nameof(SanPham.Gia)]);
        }

        private void ResetForm()
        {
            _current.Ma = 0;
            _current.Ten = "";
            _current.Gia = 0;
            dgSanPham.SelectedItem = null;
        }

        private bool HasValidationError()
        {
            return !IsValidInput();
        }

        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression[] bindings =
            {
                txtMa.GetBindingExpression(TextBox.TextProperty),
                txtTen.GetBindingExpression(TextBox.TextProperty),
                txtGia.GetBindingExpression(TextBox.TextProperty)
            };
            foreach (var b in bindings) b?.UpdateSource();

            if (HasValidationError()) return;

            _all.Add(new SanPham
            {
                Ma = _current.Ma,
                Ten = _current.Ten,
                Gia = _current.Gia
            });

            ResetForm();
            LoadPage();
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            SanPham sp = dgSanPham.SelectedItem as SanPham;
            if (sp == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa");
                return;
            }

            BindingExpression[] bindings =
            {
                txtMa.GetBindingExpression(TextBox.TextProperty),
                txtTen.GetBindingExpression(TextBox.TextProperty),
                txtGia.GetBindingExpression(TextBox.TextProperty)
            };
            foreach (var b in bindings) b?.UpdateSource();

            if (HasValidationError()) return;

            sp.Ma = _current.Ma;
            sp.Ten = _current.Ten;
            sp.Gia = _current.Gia;

            ResetForm();
            LoadPage();
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            SanPham sp = dgSanPham.SelectedItem as SanPham;
            if (sp == null) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _all.Remove(sp);
                ResetForm();
                LoadPage();
            }
        }

        private void dgSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SanPham sp = dgSanPham.SelectedItem as SanPham;
            if (sp != null)
            {
                _current.Ma = sp.Ma;
                _current.Ten = sp.Ten;
                _current.Gia = sp.Gia;
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (_pageIndex > 1) { _pageIndex--; LoadPage(); }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            int totalPage = (_all.Count + _pageSize - 1) / _pageSize;
            if (_pageIndex < totalPage) { _pageIndex++; LoadPage(); }
        }
    }
}
