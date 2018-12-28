using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace MFAX01V3
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool? bValue = value as bool?;

			if (bValue.HasValue)
				return bValue.Value ? Visibility.Visible : Visibility.Collapsed;

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Visibility? visibility = value as Visibility?;

			if (visibility.HasValue)
				return (visibility.Value == Visibility.Visible) ? true : false;

			return DependencyProperty.UnsetValue;
		}

		#endregion
	}

	public class BoolToReverseVisibilityConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool? bValue = value as bool?;

			if (bValue.HasValue)
				return bValue.Value ? Visibility.Collapsed : Visibility.Visible;

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Visibility? visibility = value as Visibility?;

			if (visibility.HasValue)
				return (visibility.Value == Visibility.Collapsed) ? true : false;

			return DependencyProperty.UnsetValue;
		}

		#endregion
	}
}
