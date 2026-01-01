using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTrain1
{
    public class MinValueConverter : IValueConverter
    {
        public double Min { get; set; }

        public object Convert(object value, Type t, object p, CultureInfo c) => value;

        public object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            if (double.TryParse(value?.ToString(), out var v) && v > Min)
                return v;

            throw new Exception($"Giá trị phải > {Min}");
        }
    }

    public class NotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type t, object p, CultureInfo c) => value;

        public object ConvertBack(object value, Type t, object p, CultureInfo c)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
                return value;

            throw new Exception("Không được để trống");
        }
    }

}
