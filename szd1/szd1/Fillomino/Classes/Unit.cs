using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace szd1.Fillomino.Classes {
	public class Unit {
		public Point Point { get; set; }
		public int Number { get; set; }
		public bool DoesHaveNumber { get { return Number > 0 ? true : false; } }
		public bool DefaultNumber { get; }
		public bool IsInVariation { get; set; }
		public int Key { get; }

		public Unit(Point point, int key, int number = 0, bool defaultNumber = false) {
			Point = point;
			Key = key;
			Number = number;
			DefaultNumber = defaultNumber;
		}
	}
}
