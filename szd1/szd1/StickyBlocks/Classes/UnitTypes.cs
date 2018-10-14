using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace szd1.StickyBlocks.Classes {

	public enum UnitType { Wall, Empty, Player, Filled, Bordered }

	public static class UnitTypes {

		public static UnitType StringToUnitType(string type) {
			switch (type) {
				case "e": return UnitType.Empty;
				case "x": return UnitType.Wall;
				case "g": return UnitType.Player;
				case "f": return UnitType.Filled;
				case "b": return UnitType.Bordered;
				default: return UnitType.Empty;
			}
		}
	}
}
