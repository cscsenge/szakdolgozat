using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace szd1.Classes {

	public class Unit {

		private SolidColorBrush DefaultColor = new SolidColorBrush((Windows.UI.Color.FromArgb(255, 255, 255, 255)));
		public Point Point { get; set; }
		public SolidColorBrush Color { get; private set; }
		public SolidColorBrush Border { get; private set; }
		public UnitType Type { get; private set; }

		public Unit(UnitType type, Point point) {
			Point = point;
			Type = type;
			//SetUnitByType(type, color);
		}

		public Unit() {

		}



		/*private void SetUnitByType(UnitType type, SolidColorBrush color) {
			if (color == null) {
				if (type == UnitType.Empty)	Border = Color = DefaultColor;
				if (type == UnitType.Wall) Border = Color = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
				return;
			}
			switch (type) {
				case UnitType.Player:
				case UnitType.Filled:
					Border = Color = color;
					break;
				case UnitType.Bordered:
					Color = DefaultColor;
					Border = color;
					break;
				case UnitType.Wall:
				case UnitType.Empty:
				default:
					Border = Color = DefaultColor;
					break;
			}
		}*/
	}
}
