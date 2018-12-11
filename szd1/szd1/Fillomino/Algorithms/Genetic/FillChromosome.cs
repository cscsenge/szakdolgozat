using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;
using Windows.Foundation;

namespace szd1.Fillomino.Algorithms.Genetic {
	public class FillChromosome {

		Random r;
		public Unit[,] FillArray { get; set; }

		public int Goodness {
			get {
				return GetGoodness();
			}
		}

		public FillChromosome(Unit[,] fillArray) {
			r = new Random();
			FillArray = fillArray;
			FillEmptyUnits();
		}

		public FillChromosome() {
			r = new Random();
			FillArray = null;
		}

		public void FillEmptyUnits() {
			for (int i = 0; i < FillArray.GetLength(1); i++) {
				for (int j = 0; j < FillArray.GetLength(0); j++) {
					if (!FillArray[i, j].DoesHaveNumber) {
						FillArray[i, j].Number = r.Next(2, 10);
					}
				}
			}
		}

		public int GetGoodness() {
			for (int i = 0; i < FillArray.GetLength(1); i++) {
				for (int j = 0; j < FillArray.GetLength(0); j++) {
					if (FillArray[i, j].DefaultNumber) {
						oks = new List<Point>();
						oks.Add(FillArray[i, j].Point);
						int subCount = NeightboursForDefaultNumber(FillArray[i, j].Point, FillArray[i, j].Number);
						count += subCount - 1;
						if (subCount == FillArray[i, j].Number) {
							sumCount++;
						}
					}
				}
			}
			return count + sumCount;
		}

		int count = 0;
		int sumCount = 0;
		List<Point> oks = new List<Point>();
		public int NeightboursForDefaultNumber(Point point, int number) {
			int x = (int)point.X;
			int y = (int)point.Y;
			if (y - 1 >= 0) {
				Unit tempUnit = FillArray[x, y - 1];
				if (tempUnit.Number == number && !oks.Contains(tempUnit.Point)) {
					if (oks.Count < number) {
						oks.Add(tempUnit.Point);
						NeightboursForDefaultNumber(tempUnit.Point, tempUnit.Number);
					}
				}
			}
			if (x - 1 >= 0) {
				Unit tempUnit = FillArray[x - 1, y];
				if (tempUnit.Number == number && !oks.Contains(tempUnit.Point)) {
					if (oks.Count < number) {
						oks.Add(tempUnit.Point);
						NeightboursForDefaultNumber(tempUnit.Point, tempUnit.Number);
					}
				}
			}
			if (x + 1 < FillArray.GetLength(1)) {
				Unit tempUnit = FillArray[x + 1, y];
				if (tempUnit.Number == number && !oks.Contains(tempUnit.Point)) {
					if (oks.Count < number) {
						oks.Add(tempUnit.Point);
						NeightboursForDefaultNumber(tempUnit.Point, tempUnit.Number);
					}
				}
			}
			if (y + 1 < FillArray.GetLength(0)) {
				Unit tempUnit = FillArray[x, y + 1];
				if (tempUnit.Number == number && !oks.Contains(tempUnit.Point)) {
					if (oks.Count < number) {
						oks.Add(tempUnit.Point);
						NeightboursForDefaultNumber(tempUnit.Point, tempUnit.Number);
					}
				}
			}
			return oks.Count;
		}
	}
}
