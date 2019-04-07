using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace szd1.Converters {
	public class CharArrayToGeometry : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			char[][] map = (char[][])value;
			ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
			if (map != null && map.Length > 0 && map.All(x => x != null)) {
				int size = map.GetLength(0);
				int unitSize = Consts.STICKY_CANVAS_WIDTH / size;
				for (int i = 0; i < size; i++) {
					int z = map[i].Length;
					for (int j = 0; j < z; j++) {
						Path pathFigure = new Path();
						switch (map[i][j]) {
							case Consts.SOKOBAN_WALL: //wall
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 51, 25, 0));
								pathFigure.StrokeThickness = 1;
								pathFigure.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
								break;
							case Consts.SOKOBAN_EMPTY: //empty space
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
								break;
							case Consts.SOKOBAN_PLAYER_FLOOR: //player
								pathFigure.Fill = new ImageBrush {
									ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/player.png"))
								};
								break;
							case Consts.SOKOBAN_EMPTY_GOAL: //empty on goal
								pathFigure.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 235, 205));
								break;
							case Consts.SOKOBAN_BOX_FLOOR: //box on floor
								pathFigure.Fill = new ImageBrush {
									ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/crate.png"))
								};
								break;
							case Consts.SOKOBAN_PLAYER_GOAL: //player on goal
								pathFigure.Fill = new ImageBrush {
									ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/player2.png"))
								};
								break;
							case Consts.SOKOBAN_BOX_GOAL: //box on goal
								pathFigure.Fill = new ImageBrush {
									ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/crate2.png"))
								};
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
