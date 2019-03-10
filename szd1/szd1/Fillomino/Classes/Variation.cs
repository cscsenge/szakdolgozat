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

		public bool HaveCommonPointWithVariation(Variation variation) {
			foreach (Point varitaionPoint in variation.VariationPoints) {
				if (VariationPoints.Exists(x => x.X == varitaionPoint.X && x.Y == varitaionPoint.Y)) {
					return true;
				}
			}
			return false;
		}

		public bool HaveCommonPointWithVariationList(List<Variation> variationList) {
			foreach (Variation variation in variationList) {
				if (HaveCommonPointWithVariation(variation)) {
					return true;
				}
			}
			return false;
		}

		public bool HaveTheSame(List<Variation> variationList) {
			foreach (Variation variation in variationList) {
				int count = 0;
				foreach (Point point in variation.VariationPoints) {
					if (VariationPoints.Contains(point)) {
						count++;
					}
				}
				if (count == Number) return true;
			}
			return false;
		}

		//public bool IsSameWithVariation(Variation variation) {
		//	int sum = 0;
		//	foreach (var variationPoint in variation.VariationPoints) {
		//		if (!VariationPoints.Exists(x => x.X == variationPoint.X && x.Y == variationPoint.Y)) {
		//			sum++;
		//		}
		//	}

		//}
	}
}
