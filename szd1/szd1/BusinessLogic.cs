using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Classes;
using szd1.GeneticAlgorithm;
using szd1.RuleBasedAlgorithm;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace szd1 {
	class BusinessLogic {
		private int stickySizeWidth;
		private int stickySizeHeight;
		private Point stickyGamer;
		private ViewModel VM;
		private static List<Point> stuckedUnits;
		public BusinessLogic(ViewModel VM) {
			this.VM = VM;
			stickyGamer = new Point();
			stuckedUnits = new List<Point>();
		}

		public Unit[,] LoadStickyBlocks(string fileName) {
			string[] rows = File.ReadAllLines(fileName);
			stickySizeWidth = int.Parse(rows[0]);
			stickySizeHeight = int.Parse(rows[1]);
			Unit[,] stickyArray = new Unit[stickySizeHeight, stickySizeWidth];
			for (int i = 0; i < stickySizeHeight; i++) {
				for (int j = 0; j < stickySizeWidth; j++) {
					string type = rows[2 + i][j].ToString();
					UnitType unitType = UnitTypes.StringToUnitType(type);
					stickyArray[i, j] = new Unit(unitType, new Point(i, j));
					if (unitType == UnitType.Player) {
						stickyGamer = new Point(i, j);
					}
				}
			}
			return stickyArray;
		}

		List<StickyPopulation> generations = new List<StickyPopulation>();
		StickyChromosome parent1 = new StickyChromosome();
		StickyChromosome parent2 = new StickyChromosome();
		StickyChromosome kid = new StickyChromosome();
		private static Random r = new Random();

		public void Start() {
			RuleBased rb = new RuleBased(VM.StickyArray);
		}

		/*public void Start() {
			StickyPopulation firstPopulation = new StickyPopulation(Consts.STICKY_PCOUNT, VM.StickyArray);
			for (int i = 0; i < Consts.STICKY_GCOUNT; i++) {
				generations.Add(null);
			}
			generations[0] = firstPopulation;

			for (int i = 0; i < Consts.STICKY_GCOUNT - 1; i++) {
				generations[i + 1] = new StickyPopulation();
				for (int j = 0; j < Consts.STICKY_PCOUNT; j++) {
					parent1 = generations[i].chromosomes[r.Next(generations[i].chromosomes.Count)];
					parent2 = generations[i].chromosomes[r.Next(generations[i].chromosomes.Count)];

					kid = Crossover();
				}
			}
		}

		public StickyChromosome Crossover() {
			StickyChromosome kid = new StickyChromosome();

		}*/

		public Unit[,] PlayerMove(Windows.UI.Core.KeyEventArgs e) {
			switch (e.VirtualKey) {
				case Windows.System.VirtualKey.Right:
					if (IsUnitCanMove(0, 1)) {
						PlayerMoveOne(0, 1);
					}
					break;
				case Windows.System.VirtualKey.Left:
					if (IsUnitCanMove(0, -1)) {
						PlayerMoveOne(0, -1);
					}
					break;
				case Windows.System.VirtualKey.Up:
					if (IsUnitCanMove(-1, 0)) {
						PlayerMoveOne(-1, 0);
					}
					break;
				case Windows.System.VirtualKey.Down:
					if (IsUnitCanMove(1, 0)) {
						PlayerMoveOne(1, 0);
					}
					break;
			}
			return VM.StickyArray;
		}

		public bool IsUnitCanMove(int dx, int dy) {
			if (VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy].Type == UnitType.Empty || VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy].Type == UnitType.Filled) {
				if (stuckedUnits.Count > 0) {
					foreach (Point stuckedUnit in stuckedUnits) {
						if (VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Empty || VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Player || VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Bordered) {
							return true;
						}
					}
				} else {
					if (VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy].Type != UnitType.Filled) return true;
				}
			}
			return false;
		}

		public void PlayerMoveOne(int dx, int dy) {
			AddNewFilledOnes();
			VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y] = new Unit(UnitType.Empty, new Point(stickyGamer.X, stickyGamer.Y));
			stickyGamer.X += dx;
			stickyGamer.Y += dy;
			VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y] = new Unit(UnitType.Player, new Point(stickyGamer.X, stickyGamer.Y)); ;
			List<Point> tempStuckedUnits = new List<Point>();
			foreach (Point stuckedUnit in stuckedUnits) {
				if (VM.StickyArray[(int)stuckedUnit.X, (int)stuckedUnit.Y].Type != UnitType.Player) VM.StickyArray[(int)stuckedUnit.X, (int)stuckedUnit.Y] = new Unit(UnitType.Empty, new Point(stuckedUnit.X, stuckedUnit.Y));
				if (VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Bordered) {
					VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] = new Unit(UnitType.Empty, new Point(stuckedUnit.X + dx, stuckedUnit.Y + dy));
					break;
				}
				Point temp = new Point(stuckedUnit.X, stuckedUnit.Y);
				temp.X += dx;
				temp.Y += dy;
				tempStuckedUnits.Add(temp);
				VM.StickyArray[(int)temp.X, (int)temp.Y] = new Unit(UnitType.Filled, new Point(temp.X, temp.Y)); ;
			}
			stuckedUnits = tempStuckedUnits;
		}

		public void AddNewFilledOnes() {
			if (VM.StickyArray[(int)stickyGamer.X + 1, (int)stickyGamer.Y].Type == UnitType.Filled && !stuckedUnits.Contains(new Point(stickyGamer.X + 1, stickyGamer.Y))) {
				stuckedUnits.Add(new Point(stickyGamer.X + 1, stickyGamer.Y));
			} else if (VM.StickyArray[(int)stickyGamer.X - 1, (int)stickyGamer.Y].Type == UnitType.Filled && !stuckedUnits.Contains(new Point(stickyGamer.X - 1, stickyGamer.Y))) {
				stuckedUnits.Add(new Point(stickyGamer.X - 1, stickyGamer.Y));
			} else if (VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y + 1].Type == UnitType.Filled && !stuckedUnits.Contains(new Point(stickyGamer.X, stickyGamer.Y + 1))) {
				stuckedUnits.Add(new Point(stickyGamer.X, stickyGamer.Y + 1));
			} else if (VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y - 1].Type == UnitType.Filled && !stuckedUnits.Contains(new Point(stickyGamer.X, stickyGamer.Y - 1))) {
				stuckedUnits.Add(new Point(stickyGamer.X, stickyGamer.Y - 1));
			}
		}
	}
}
