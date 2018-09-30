using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace szd1.Converters {
	class MapToGeometryConverter: IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			string[,] table = (string[,])value;
			ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
			if (table != null) {
				int unitSize = Consts.STICKY_CANVAS_WIDTH / table.GetLength(1);
				for (int i = 0; i < table.GetLength(0); i++) {
					for (int j = 0; j < table.GetLength(1); j++) {
						Path pathFigure = new Path();
						switch (table[i, j]) {
							case "x":
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
								break;
							case "e":
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
								break;
							case "g":
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
								break;
							case "b":
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
								pathFigure.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 128, 0));
								pathFigure.StrokeThickness = 2;
								break;
							case "f":
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 128, 0));
								break;
						}
						RectangleGeometry rectangle = new RectangleGeometry();
						rectangle.Rect = new Windows.Foundation.Rect(j * unitSize, i * unitSize, unitSize, unitSize);
						pathFigure.Data = rectangle;
						shapes.Add(pathFigure);
					}
				}
			}
			return shapes;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}
	}
}
