using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace szd1.StickyBlocks.Classes {

	public class Unit {

		private SolidColorBrush DefaultColor = new SolidColorBrush((Windows.UI.Color.FromArgb(255, 255, 255, 255)));
		public Point Point { get; set; }
		public SolidColorBrush Color { get; private set; }
		public SolidColorBrush Border { get; private set; }
		public UnitType Type { get; private set; }

		public Unit(UnitType type, Point point) {
			Point = point;
			Type = type;
		}

		public Unit() {

		}
	}
}
