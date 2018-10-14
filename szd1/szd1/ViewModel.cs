using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using szd1.StickyBlocks;
using szd1.StickyBlocks.Classes;

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
		public StickyBusinessLogic BL;

		public ViewModel() {
			BL = new StickyBusinessLogic(this);
		}

		public void LoadStickyBlocks(string fileName) {
			StickyArray = BL.LoadStickyBlocks(fileName);
		}

		public void StickyUnitMove(Windows.UI.Core.KeyEventArgs args) { //todo UGLY!!!
			StickyArray = BL.PlayerMove(args);
		}
	}

	abstract class Bindable : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public void OPC([CallerMemberName] string n = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
		}
	}
}
