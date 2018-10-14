using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.StickyBlocks.Classes;

namespace szd1.StickyBlocks.Algorithms.GeneticAlgorithm {
	class StickyChromosome {
		private static Random r = new Random();
		internal Unit[,] Table { get; set; }

		public StickyChromosome() {

		}

		public StickyChromosome(Unit[,] table) {
			Table = table;
		}

		public StickyChromosome(int width, int height) {
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					Table[i, j] = null;
				}
			}
		}

		public double GetDistance() {
			Unit player = new Unit();
			foreach (Unit unitItem in Table) {
				if (unitItem.Type == UnitType.Player) {
					player = unitItem;
				}
			}
			Unit filledUnit = GetClosestFilledUnit();
			int width = Table.GetLength(1);
			return Math.Abs(((player.Point.X + 1) * width + player.Point.Y) - ((filledUnit.Point.X + 1) * width + filledUnit.Point.Y));
		}

		public Unit GetClosestFilledUnit() {
			//todo
			foreach (Unit unitTtem in Table) {
				if (unitTtem.Type == UnitType.Filled) {
					return unitTtem;
				}
			}
			return null;
		}
	}
}
