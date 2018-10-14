using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using szd1.Classes;

namespace szd1 {
	class ViewModel: Bindable {
		bool isInMenu = true;
		public bool IsInMenu { get { return isInMenu; } set { isInMenu = value; OPC(); } }
		bool isInFillomino = false;
		public bool IsInFillomino { get { return isInFillomino; } set { isInFillomino = value; OPC(); } }
		bool isInSticky = false;
		public bool IsInSticky { get { return isInSticky; } set { isInSticky = value; OPC(); } }
		Unit[,] stickyArray;
		public Unit[,] StickyArray { get { return stickyArray; } set { stickyArray = value; OPC(); } }
		public BusinessLogic BL;

		public ViewModel() {
			BL = new BusinessLogic(this);
		}

		public void LoadStickyBlocks(string fileName) {
			StickyArray = BL.LoadStickyBlocks(fileName);
			GetDistance();
		}

		public void StickyUnitMove(Windows.UI.Core.KeyEventArgs args) { //todo UGLY!!!
			StickyArray = BL.PlayerMove(args);
		}

		public void GetDistance() {
			Unit player = new Unit();
			foreach (Unit unitItem in StickyArray) {
				if (unitItem.Type == UnitType.Player) {
					player = unitItem;
				}
			}
			Unit filledUnit = GetClosestFilledUnit();
			int width = StickyArray.GetLength(1);
			double distance = Math.Abs(((player.Point.X + 1) * width + player.Point.Y) - ((filledUnit.Point.X + 1) * width + filledUnit.Point.Y));
		}

		public Unit GetClosestFilledUnit() {
			//todo
			foreach (Unit unitTtem in StickyArray) {
				if (unitTtem.Type == UnitType.Filled) {
					return unitTtem;
				}
			}
			return null;
		}
	}

	abstract class Bindable : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public void OPC([CallerMemberName] string n = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
		}
	}
}
