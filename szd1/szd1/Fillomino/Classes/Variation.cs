using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace szd1.Fillomino.Classes {
	public class Variation {
		public List<Point> VariationPoints { get; set; }
		public int Number { get; set; }

		public Variation(List<Point> points, int number) {
			VariationPoints = new List<Point>();
			foreach (Point point in points) {
				VariationPoints.Add(point);
			}
			Number = number;
		}
	}
}
