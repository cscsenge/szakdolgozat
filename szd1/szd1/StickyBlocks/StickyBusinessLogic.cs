using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.StickyBlocks.Classes;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using szd1.StickyBlocks.Algorithms;
using Windows.UI.Xaml.Controls;

namespace szd1.StickyBlocks {

	class StickyBusinessLogic {
		private int stickySizeWidth;
		private int stickySizeHeight;
		private Point stickyGamer;
		private ViewModel VM;
		private static List<Point> stuckedUnits;
		public StickyBusinessLogic(ViewModel VM) {
			this.VM = VM;
			stickyGamer = new Point();
			stuckedUnits = new List<Point>();
		}

		public void LoadStickyBlocks(string fileName) {
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
			VM.StickyArray = stickyArray;
			VM.IsItTheEnd = false;
		}

		public void SetComboBoxes(ComboBox levelChooser, ComboBox algorithmChooser) {
			levelChooser.Items.Clear();
			algorithmChooser.Items.Clear();
			DirectoryInfo dir = new DirectoryInfo(@"Levels\StickyBlocks\");
			foreach (FileInfo file in dir.GetFiles()) {
				if (file.Extension.Contains("txt")) {
					levelChooser.Items.Add(file.Name);
				}
			}
			foreach (string algorithm in Consts.STICKY_ALGORITHMS) {
				algorithmChooser.Items.Add(algorithm);
			}
		}

		public void Start() {
			Algorithms.RuleBased.RuleBased rb = new Algorithms.RuleBased.RuleBased(VM.StickyArray);
		}

		public void PlayerMove(Windows.UI.Core.KeyEventArgs e) {
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
		}

		public bool IsUnitCanMove(int dx, int dy) {
			if (VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy].Type == UnitType.Empty || VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy].Type == UnitType.Filled) {
				if (stuckedUnits.Count > 0) {
					foreach (Point stuckedUnit in stuckedUnits) {
						if (VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Empty || VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Player || VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Bordered) {
							return true; //TODO if its in stuckedunits
						}
					}
				} else {
					if (VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy].Type != UnitType.Filled) return true;
				}
			}
			return false;
		}

		public void PlayerMoveOne(int dx, int dy) {
			Unit[,] stickyArray = VM.StickyArray;
			AddNewFilledOnes();
			stickyArray[(int)stickyGamer.X, (int)stickyGamer.Y] = new Unit(UnitType.Empty, new Point(stickyGamer.X, stickyGamer.Y));
			stickyGamer.X += dx;
			stickyGamer.Y += dy;
			stickyArray[(int)stickyGamer.X, (int)stickyGamer.Y] = new Unit(UnitType.Player, new Point(stickyGamer.X, stickyGamer.Y)); ;
			List<Point> tempStuckedUnits = new List<Point>();
			foreach (Point stuckedUnit in stuckedUnits) {
				if (stickyArray[(int)stuckedUnit.X, (int)stuckedUnit.Y].Type != UnitType.Player) stickyArray[(int)stuckedUnit.X, (int)stuckedUnit.Y] = new Unit(UnitType.Empty, new Point(stuckedUnit.X, stuckedUnit.Y));
				if (stickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy].Type == UnitType.Bordered) { //if it gets to its bordered unit
					stickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] = new Unit(UnitType.Empty, new Point(stuckedUnit.X + dx, stuckedUnit.Y + dy));
					break;
				}
				Point temp = new Point(stuckedUnit.X, stuckedUnit.Y);
				temp.X += dx;
				temp.Y += dy;
				tempStuckedUnits.Add(temp);
				stickyArray[(int)temp.X, (int)temp.Y] = new Unit(UnitType.Filled, new Point(temp.X, temp.Y)); ;
			}
			stuckedUnits = tempStuckedUnits;
			VM.StickyArray = stickyArray;
			if (IsItTheEnd()) {
				VM.IsItTheEnd = true;
			}
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

		public bool IsItTheEnd() {
			foreach (Unit item in VM.StickyArray) {
				if (item.Type == UnitType.Bordered) return false;
			}
			return true;
		}
	}
}
