using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public string[,] LoadStickyBlocks(string fileName) {
			string[] rows = File.ReadAllLines(fileName);
			stickySizeWidth = int.Parse(rows[0]);
			stickySizeHeight = int.Parse(rows[1]);
			string[,] stickyArray = new string[stickySizeHeight, stickySizeWidth];
			for (int i = 0; i < stickySizeHeight; i++) {
				for (int j = 0; j < stickySizeWidth; j++) {
					stickyArray[i, j] = rows[2 + i][j].ToString();
					if (stickyArray[i, j] == "g") {
						stickyGamer = new Point(i, j);
					}
				}
			}
			return stickyArray;
		}

		public string[,] PlayerMove(Windows.UI.Core.KeyEventArgs e) {
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
			if (VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy] == "e" || VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy] == "f") {
				if (stuckedUnits.Count > 0) {
					foreach (Point stuckedUnit in stuckedUnits) {
						if (VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] == "e" || VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] == "g" || VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] == "b") {
							return true;
						}
					}
				} else {
					if (VM.StickyArray[(int)stickyGamer.X + dx, (int)stickyGamer.Y + dy] != "f") return true;
				}
			}
			return false;
		}

		public void PlayerMoveOne(int dx, int dy) {
			AddNewFilledOnes();
			VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y] = "e";
			stickyGamer.X += dx;
			stickyGamer.Y += dy;
			VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y] = "g";
			List<Point> tempStuckedUnits = new List<Point>();
			foreach (Point stuckedUnit in stuckedUnits) {
				if (VM.StickyArray[(int)stuckedUnit.X, (int)stuckedUnit.Y] != "g") VM.StickyArray[(int)stuckedUnit.X, (int)stuckedUnit.Y] = "e";
				if (VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] == "b") {
					VM.StickyArray[(int)stuckedUnit.X + dx, (int)stuckedUnit.Y + dy] = "e";
					break;
				}
				Point temp = new Point(stuckedUnit.X, stuckedUnit.Y);
				temp.X += dx;
				temp.Y += dy;
				tempStuckedUnits.Add(temp);
				VM.StickyArray[(int)temp.X, (int)temp.Y] = "f";
			}
			stuckedUnits = tempStuckedUnits;
		}

		public void AddNewFilledOnes() {
			if (VM.StickyArray[(int)stickyGamer.X + 1, (int)stickyGamer.Y] == "f" && !stuckedUnits.Contains(new Point(stickyGamer.X + 1, stickyGamer.Y))) {
				stuckedUnits.Add(new Point(stickyGamer.X + 1, stickyGamer.Y));
			} else if (VM.StickyArray[(int)stickyGamer.X - 1, (int)stickyGamer.Y] == "f" && !stuckedUnits.Contains(new Point(stickyGamer.X - 1, stickyGamer.Y))) {
				stuckedUnits.Add(new Point(stickyGamer.X - 1, stickyGamer.Y));
			} else if (VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y + 1] == "f" && !stuckedUnits.Contains(new Point(stickyGamer.X, stickyGamer.Y + 1))) {
				stuckedUnits.Add(new Point(stickyGamer.X, stickyGamer.Y + 1));
			} else if (VM.StickyArray[(int)stickyGamer.X, (int)stickyGamer.Y - 1] == "f" && !stuckedUnits.Contains(new Point(stickyGamer.X, stickyGamer.Y - 1))) {
				stuckedUnits.Add(new Point(stickyGamer.X, stickyGamer.Y - 1));
			}
		}
	}
}
