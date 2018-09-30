using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace szd1.Converters {
	class BoolToVisibilityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			bool ertek = (bool)value;
			if (ertek)
				return Visibility.Visible;
			else return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}
	}
}
